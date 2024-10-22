using Backend.Models;

public class AuthService
{
    private readonly JwtServices _jwServices;

    public AuthService(JwtServices jwServices)
    {
        _jwServices = jwServices;
    }

    
    public (string token, User user) Authenticate(LoginModel loginModel)
    {
        if (_jwServices.ValidateUser(loginModel, out var user))
        {
            var token = _jwServices.GenerateToken(user);
            return (token, user); 
        }
        return (null, null); 
    }
}
