using Microsoft.AspNetCore.Identity;
using RSS_Application.Models.Database;
using RSS_Application.Role;

namespace RSS_Application.Services.Initializer.Auth;

public class UsersInitializer
{
    public static async Task AdminInitializeAsync(UserManager<User> userManager, string name, string password)
    {
        if (await userManager.FindByNameAsync(name) is null)
        {
            User admin = new() { UserName = name, RegisteredTime = DateTime.Now };
            IdentityResult result = await userManager.CreateAsync(admin, password);
            if (result.Succeeded)
                await userManager.AddToRoleAsync(admin, Roles.Admin);
        }
    }
}