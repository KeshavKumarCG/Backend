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
                                where user.Role.RoleType == 3 
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
