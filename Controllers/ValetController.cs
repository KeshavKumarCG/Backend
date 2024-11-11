using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/valet")]
    [ApiController]
    public class ValetController : ControllerBase
    {
        private readonly CarParkingSystem _context;

        public ValetController(CarParkingSystem context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<object>>> GetValets()
        {
            var valets = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.Role != null && u.Role.ID == 2)
                .Select(u => new
                {
                    u.Name,
                    u.PhoneNumber,
                    u.Email
                })
                .ToListAsync();

            return Ok(valets);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetValetById(int id)
        {
            var valet = await _context.Users
                .Include(u => u.Role)
                .Where(u => u.ID == id && u.Role != null && u.Role.ID == 2)
                .Select(u => new
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
