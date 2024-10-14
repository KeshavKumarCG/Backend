using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Backend.Services
{
    public class AuthService
    {
        private readonly ValetParkingDbContext _context;
        private readonly JwtService _jwtService;

        public AuthService(ValetParkingDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<(string, User)> AuthenticateAsync(LoginRequest loginRequest)
        {
            var user = await _context.Users
                
                .FirstOrDefaultAsync(u =>
                    (u.Email == loginRequest.EmailOrPhone || u.PhoneNumber.ToString() == loginRequest.EmailOrPhone)
                    && u.Password == loginRequest.Password);

            if (user == null)
                return (null, null);

            var token = _jwtService.GenerateJwtToken(user);
            return (token, user);
        }
    }
}
