using Backend.Models;
using Microsoft.AspNetCore.Http; 
using System.Security.Claims;

public class AuthService
{
    private readonly JwtServices _jwServices;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(JwtServices jwServices, IHttpContextAccessor httpContextAccessor)
    {
        _jwServices = jwServices;
        _httpContextAccessor = httpContextAccessor;
    }

    public (string token, User user) Authenticate(LoginModel loginModel)
    {
        if (_jwServices.ValidateUser(loginModel, out var user))
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var token = _jwServices.GenerateToken(user);
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session == null)
            {
                throw new InvalidOperationException("Session is not available");
            }

            session.SetInt32("UserID", user.ID);
            session.SetString("Email", user.Email);
            session.SetString("Role", user.Role switch
            {
                1 => "Admin",
                2 => "Valet",
                3 => "User",
                _ => "Unknown"
            });

            return (token, user);
        }
        return (null, null);
    }
}
