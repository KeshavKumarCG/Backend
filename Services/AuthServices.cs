using Backend.Models;

public class AuthService
{
    private readonly JwtServices _jwServices;

    public AuthService(JwtServices jwServices)
    {
        _jwServices = jwServices;
    }

    // Updated to return a tuple with both token and user
    public (string token, User user) Authenticate(LoginModel loginModel)
    {
        if (_jwServices.ValidateUser(loginModel, out var user))
        {
            var token = _jwServices.GenerateToken(user);
            return (token, user); // Return both token and user
        }
        return (null, null); // Return nulls if authentication fails
    }
}
