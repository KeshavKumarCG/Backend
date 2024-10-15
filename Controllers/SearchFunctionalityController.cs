using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend.Data; // Ensure this matches your context's namespace
using Backend.Models; // Include the namespace for your models
using System.Threading.Tasks;

namespace Backend.Controllers // Adjust to your project namespace
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
                                where user.Role == true // Corrected to compare with true
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
    }
}