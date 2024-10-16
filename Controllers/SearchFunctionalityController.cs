
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchFunctionalityController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public SearchFunctionalityController(CarParkingContext context)
        {
            _context = context;
        }

        [HttpGet("combined")]
        public async Task<IActionResult> GetCombinedData()
        {
            var result = await (from car in _context.Cars
                                join status in _context.CarStatus on car.StatusID equals status.ID
                                join user in _context.Users on car.OwnerID equals user.ID
                                where user.Role == true
                                select new
                                {
                                    CarID = car.ID,
                                    CarModel = car.CarModel,
                                    CarNumber = car.CarNumber,
                                    PhoneNumber = user.PhoneNumber,
                                    Status = status.Status
                                }).ToListAsync();

            return Ok(result);
        }

 [HttpPatch("updateStatus/{carId}")]
public async Task<IActionResult> UpdateCarStatus(string carId, [FromBody] CarStatusUpdateDto updateDto)
{
    if (string.IsNullOrEmpty(carId) || updateDto == null || string.IsNullOrEmpty(updateDto.NewStatus))
    {
        return BadRequest("Invalid input data");
    }

    var car = await _context.Cars.FindAsync(carId);
    if (car == null)
    {
        return NotFound("Car not found");
    }

    var status = await _context.CarStatus
        .FirstOrDefaultAsync(s => s.Status != null && s.Status.ToLower() == updateDto.NewStatus.ToLower());
    if (status == null)
    {
        return BadRequest("Invalid status");
    }

    car.StatusID = status.ID;
    car.UpdatedAt = DateTime.UtcNow;
    car.UpdatedBy = "System"; // Or the actual user making the update

    var statusLog = new CarStatusLog
    {
        CarID = car.ID,
        StatusID = status.ID,
        ChangedAt = DateTime.UtcNow
    };
    _context.CarStatusLogs.Add(statusLog);

    try
    {
        await _context.SaveChangesAsync();
        return Ok("Car status updated successfully");
    }
    catch (Exception ex)
    {
        // Log the exception
        return StatusCode(500, "An error occurred while updating the car status");
    }
}

    }
}
