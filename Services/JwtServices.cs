using Backend.Data;
using Backend.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Backend
{
    public class JWTService
    {
        private readonly string _secretKey;
        private readonly CarParkingContext _context;

        public JWTService(IConfiguration configuration, CarParkingContext context)
        {
            _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "JWT Secret Key is not configured.");
            _context = context ?? throw new ArgumentNullException(nameof(context), "Database context is not configured.");
        }

        public string GenerateJwtToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (string.IsNullOrEmpty(_secretKey))
            {
                throw new ArgumentException("JWT Secret Key is not configured.");
            }

            var role = user.Role != null ?
                (user.Role.RoleType switch
                {
                    1 => "Admin",
                    2 => "Valet",
                    3 => "User",
                    _ => "Unknown"
                })
                : "Unknown";

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "yourapp",
                audience: "yourapp",
                claims: claims,
                expires: DateTime.Now.AddHours(72),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateUser(LoginModel loginModel, out User? user)
        {
            user = null;

            if (loginModel == null || string.IsNullOrEmpty(loginModel.EmailOrPhone) || string.IsNullOrEmpty(loginModel.Password))
            {
                return false;
            }

            user = _context.Users
                .FirstOrDefault(u => (u.Email == loginModel.EmailOrPhone || u.PhoneNumber == loginModel.EmailOrPhone));

            if (user != null)
            {
                if (user.Password != loginModel.Password)
                {
                    user = null;
                    return false;
                }
            }

            return user != null;
        }
    }
}
