using Backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Add ApiController attribute for automatic model validation and other benefits
    public class AuthController : ControllerBase // Use ControllerBase for API controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Authenticate user
            var (token, user) = _authService.Authenticate(loginModel);
            if (token == null || user == null)
                return Unauthorized();

            // Set claims for the authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ? "User" : "Valet") // Assuming user.Role is a bool
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Remembers the user after they close the browser
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1) // Set token expiration
            };

            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            // Return response with token and user details
            return Ok(new
            {
                Token = token,
                Name = user.Email,
                Role = user.Role ? "User" : "Valet" // Return role as string
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok(new { Message = "Logged out successfully" });
        }
    }
}
