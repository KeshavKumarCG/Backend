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
            var token = _jwServices.GenerateToken(user);
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetInt32("UserID", user.ID);
            session.SetString("Email", user.Email);
            session.SetString("Role", user.Role ? "User" : "Valet");

            return (token, user);
        }
        return (null, null);
    }
}
