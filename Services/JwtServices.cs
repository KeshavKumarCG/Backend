using Backend.Data;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtServices
{
    private readonly CarParkingContext _context;
    private readonly string _jwtKey;
    private readonly string _jwtIssuer;
    private readonly string _jwtAudience;

    public JwtServices(CarParkingContext context)
    {
        _context = context;
        _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
        _jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        _jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
    }

    public string GenerateToken(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtKey);

        var roleName = _context.Roles
            .Where(r => r.RoleID == user.RoleID)
            .Select(r => r.RoleName)
            .FirstOrDefault() ?? throw new InvalidOperationException("Role not found.");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, roleName)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = _jwtIssuer,
            Audience = _jwtAudience,
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
