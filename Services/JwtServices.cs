using Backend.Data;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


public class JwtServices
{
    private readonly CarParkingContext _context;
    private readonly IConfiguration _configuration;

    public JwtServices(CarParkingContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key is not configured."));

        string role = user.Role switch
        {
            1 => "Admin",
            2 => "Valet",
            3 => "User",
            _ => throw new InvalidOperationException("Invalid role.")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, role)
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
        user = _context.Users.FirstOrDefault(u =>
            u.Email == loginModel.EmailOrPhone || u.PhoneNumber == loginModel.EmailOrPhone);

        if (user == null)
        {
            return false;
        }

        return loginModel.Password == user.Password; 
    }
}
