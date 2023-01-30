using RSS_Application.Models.Account;

namespace RSS_Application.Services.AuthServices;

public interface IAuthService
{
    Task LoginUserAsync(LoginModel model);
    Task RegisterUserAsync(RegisterModel model);
    Task LogoutUserAsync();
}