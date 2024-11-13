using Backend.Data;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly CarParkingContext _context;
        private readonly JWTService _jwtService;

        public AuthController(CarParkingContext context, JWTService jwtService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel request)
        {
            if (request == null)
            {
                return BadRequest("Invalid login request.");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .Where(u => (u.Email == request.EmailOrPhone || u.PhoneNumber == request.EmailOrPhone) && u.Password == request.Password)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _jwtService.GenerateJwtToken(user);

            var session = HttpContext.Session;
            session.SetInt32("UserID", user.ID); 
            session.SetString("Email", user.Email);
            session.SetInt32("Role", user.Role?.RoleType ?? 0);

            return Ok(new { token, userId = user.ID, email = user.Email, role = user.Role?.RoleType ?? 0 });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("AuthToken");
            HttpContext.Session.Clear();

            return Ok();
        }
    }
}
