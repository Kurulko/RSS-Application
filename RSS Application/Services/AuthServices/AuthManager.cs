using Microsoft.AspNetCore.Identity;
using RSS_Application.Models.Account;
using RSS_Application.Models.Database;
using RSS_Application.Role;

namespace RSS_Application.Services.AuthServices;

public class AuthManager : IAuthService
{
    readonly SignInManager<User> signInManager;
    readonly UserManager<User> userManager;
    public AuthManager(SignInManager<User> signInManager, UserManager<User> userManager)
        => (this.signInManager, this.userManager) = (signInManager, userManager);

    public async Task LoginUserAsync(LoginModel login)
    {
        SignInResult res = await signInManager.PasswordSignInAsync(login.Name, login.Password, login.RememberMe, false);
        if (!res.Succeeded)
            throw new Exception("Password or/and login invalid");
    }

    public async Task RegisterUserAsync(RegisterModel register)
    {
        User user = (User)register;
        user.RegisteredTime = DateTime.Now;
        IdentityResult result = await userManager.CreateAsync(user, register.Password);
        if (result.Succeeded)
        {
            await signInManager.SignInAsync(user, register.RememberMe);
            await userManager.AddToRolesAsync(user, new List<string>() { Roles.User });
        }
        else
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
    }

    public async Task LogoutUserAsync()
        => await signInManager.SignOutAsync();
}
