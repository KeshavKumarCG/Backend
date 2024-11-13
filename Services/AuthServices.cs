using Backend;
using Backend.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Backend.Services
{
    public class AuthService
    {
        private readonly JWTService _jwServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(JWTService jwServices, IHttpContextAccessor httpContextAccessor)
        {
            _jwServices = jwServices;
            _httpContextAccessor = httpContextAccessor;
        }

        public (string token, User user) Authenticate(LoginModel loginModel)
        {
            if (_jwServices.ValidateUser(loginModel, out var user))
            {
                var token = _jwServices.GenerateJwtToken(user);
                var session = _httpContextAccessor.HttpContext?.Session;
                if (session != null)
                {
                    session.SetInt32("UserID", user.ID);
                    session.SetString("Email", user.Email);
                    session.SetString("Role", user.Role.ToString());
                }
                return (token, user);
            }
            return (string.Empty, null);
        }
    }
}
