using Backend.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;
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
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var (token, user) = _authService.Authenticate(loginModel);
            if (token == null || user == null)
                return Unauthorized();
 
          
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role ? "User" : "Valet"), 
                new Claim("UserID", user.ID.ToString()) 
            };
 
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
 
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, 
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
            };
 
            // Sign in the user
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
 
            // Return response with token and user details
            return Ok(new
            {
                Token = token,
                Name = user.Email,
                Role = user.Role ? "User" : "Valet", 
                ID = user.ID 
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

