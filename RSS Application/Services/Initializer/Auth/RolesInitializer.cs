using Microsoft.AspNetCore.Identity;
using RSS_Application.Role;

namespace RSS_Application.Services.Initializer.Auth;

public class RolesInitializer
{
    static async Task<bool> HasRoleAsync(RoleManager<IdentityRole> roleManager, string name)
            => await roleManager.FindByNameAsync(name) is not null;
    static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string name)
        => await roleManager.CreateAsync(new IdentityRole(name));
    public static async Task InitializeAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = { Roles.Admin, Roles.User };
        foreach (string role in roles)
            if (!await HasRoleAsync(roleManager, role))
                await CreateRoleAsync(roleManager, role);
    }
}
