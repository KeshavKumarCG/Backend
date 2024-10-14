using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.EmailOrPhone) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Invalid login request.");
            }

            var (token, user) = await _authService.AuthenticateAsync(loginRequest);

            if (token == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            // Return token and user details
            return Ok(new
            {
                Token = token,
                User = new
                {
                    user.ID,
                    user.CYGID,
                    user.Name,
                    user.PhoneNumber,
                    user.Email,
                    user.Role,
                }
            });
        }
    }
}
