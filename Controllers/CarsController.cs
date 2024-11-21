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
        public async Task<IActionResult> UpdateCarStatus([FromBody] CarStatusUpdateDto request)
        {
            if (string.IsNullOrEmpty(request.id) || string.IsNullOrEmpty(request.statusId))
            {
                return BadRequest(new { Message = "ID and StatusId are required" });
            }

            try
            {
                // Update car status directly
                var rowsAffected = await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE Cars SET StatusID = {0}, UpdatedAt = GETDATE(), UpdatedBy = {1} WHERE ID = {2}",
                    request.statusId, "system", request.id);

                if (rowsAffected == 0)
                {
                    return NotFound(new
                    {
                        id = request.id,
                        statusId = request.statusId,
                        Message = "Car not found"
                    });
                }

                // Insert into CarStatusLog
                await _context.Database.ExecuteSqlRawAsync(
                    @"INSERT INTO CarStatusLog (CarID, StatusID, UserID, CreatedAt, UpdatedAt) 
                    VALUES ({0}, {1}, {2}, GETDATE(), GETDATE())",
                    request.id, request.statusId, 1);

                return Ok(new
                {
                    id = request.id,
                    statusId = request.statusId,
                    Message = "Car status updated successfully"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating car status", Error = ex.Message });
            }
        }
    }

    public class CarStatusUpdateDto
    {
        public string id { get; set; }
        public string statusId { get; set; }
    }
}
