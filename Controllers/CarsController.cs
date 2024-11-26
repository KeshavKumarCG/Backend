
using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public CarsController(CarParkingContext context)
        {
            _context = context;
        }

        [HttpPatch]
        public async Task<IActionResult> UpdateCarStatus([FromBody] CarPatchRequest request)
        {
            if (string.IsNullOrEmpty(request.StatusID) || (string.IsNullOrEmpty(request.CarID) && string.IsNullOrEmpty(request.CarNumber)))
            {
                return BadRequest(new { Message = "StatusID and either CarID or CarNumber are required" });
            }

            try
            {
                int rowsAffected = 0;

                if (!string.IsNullOrEmpty(request.CarID))
                {
                    rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                        @"UPDATE Cars 
                          SET StatusID = {0}, UpdatedAt = GETDATE(), UpdatedBy = {1} 
                          WHERE ID = {2}",
                        request.StatusID, "system", request.CarID);
                }
               
                else if (!string.IsNullOrEmpty(request.CarNumber))
                {
                    rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                        @"UPDATE Cars 
                          SET StatusID = {0}, UpdatedAt = GETDATE(), UpdatedBy = {1} 
                          WHERE CarNumber = {2}",
                        request.StatusID, "system", request.CarNumber);
                }

                if (rowsAffected == 0)
                {
                    return NotFound(new
                    {
                        CarID = request.CarID,
                        CarNumber = request.CarNumber,
                        StatusID = request.StatusID,
                        Message = "Car not found"
                    });
                }

                // Update the CarStatusLog
                // await _context.Database.ExecuteSqlRawAsync(
                //     @"INSERT INTO CarStatusLog (CarID, StatusID, UpdatedAt, UpdatedBy) 
                //       VALUES ({0}, {1}, GETDATE(), {2})",
                //     request.CarID ?? "UNKNOWN", request.StatusID, "system");

                return Ok(new
                {
                    CarID = request.CarID,
                    CarNumber = request.CarNumber,
                    StatusID = request.StatusID,
                    Message = "Car status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating car status", Error = ex.Message });
            }
        }
    }
}
