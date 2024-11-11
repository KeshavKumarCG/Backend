using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtServices
{
    private readonly CarParkingSystem _context;
    private readonly IConfiguration _configuration;

    public JwtServices(CarParkingSystem context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured."));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role?.ID.ToString() ?? "Unknown") // Handle role with fallback
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateUser(LoginModel loginModel, out User? user)
    {
        user = _context.Users
            .Include(u => u.Role)
            .FirstOrDefault(u =>
                u.Email == loginModel.EmailOrPhone || u.PhoneNumber == loginModel.EmailOrPhone);

        if (user == null)
        {
            return false;
        }

        // Simple plain-text password comparison
        return loginModel.Password == user.Password;
    }
}
