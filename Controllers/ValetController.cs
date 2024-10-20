using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValetController : ControllerBase
    {
        private readonly CarParkingContext _context;

        public ValetController(CarParkingContext context)
        {
            _context = context;
        }

        // GET: api/valet
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetValets()
        {
            var valets = await _context.Users
                .Where(u => u.Role == false)  // Role false indicates valet
                .Select(u => new               // Project only specific fields
                {
                    u.Name,
                    u.PhoneNumber,
                    u.Email
                })
                .ToListAsync();

            return Ok(valets);
        }

        // GET: api/valet/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetValetById(int id)
        {
            var valet = await _context.Users
                .Where(u => u.ID == id && u.Role == false)
                .Select(u => new               // Project only specific fields
                {
                    u.Name,
                    u.PhoneNumber,
                    u.Email
                })
                .FirstOrDefaultAsync();

            if (valet == null)
            {
                return NotFound();
            }

            return Ok(valet);
        }
    }
}
