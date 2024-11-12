using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public CarsController(CarParkingContext context)
        {
            _context = context;
        }

        // PATCH api/cars/update-status
        [HttpPatch("update-status")]
        public async Task<IActionResult> UpdateCarStatus([FromBody] UpdateCarStatusRequest request)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.ID == request.CarID);

            if (car == null)
            {
                return NotFound("Car not found");
            }

            car.StatusID = request.StatusID;
            car.UpdatedAt = DateTime.UtcNow;

            _context.Cars.Update(car);
            await _context.SaveChangesAsync();

            return Ok(car);
        }
    }

    // Define a separate request model for updating car status
    public class UpdateCarStatusRequest
    {
        public string CarID { get; set; }
        public string StatusID { get; set; }
    }
}
