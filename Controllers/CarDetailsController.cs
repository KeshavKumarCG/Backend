using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarDetailsController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public CarDetailsController(CarParkingContext context)
        {
            _context = context;
        }

        // GET: api/cardetails/user/{userId}
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CarDetails>>> GetCarDetailsByUserId(int id)
        {
            var cars = await _context.Cars
                .Where(c => c.OwnerID == id)
                .Join(
                    _context.CarStatus,            // Joining with CarStatus table
                    car => car.StatusID,            // Foreign key in Cars
                    status => status.ID,            // Primary key in CarStatus
                    (car, status) => new CarDetails // Projecting results
                    {
                        ID = car.ID,
                        CarNumber = car.CarNumber,
                        CarModel = car.CarModel,
                        Status = status.Status       // Getting car status
                    })
                .ToListAsync();

            if (cars == null || !cars.Any())
            {
                return NotFound(); // Return 404 if no cars found
            }

            return Ok(cars); // Return the list of car details with status
        }
    }
}
