using Microsoft.AspNetCore.Identity;

namespace Reminders.DAL.Data;

public class RoleInitializer
{
    public static async Task RoleInitializeAsync(RoleManager<IdentityRole<int>> roleManager)
    {
        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
        }

        if (await roleManager.FindByNameAsync("User") == null)
        {
            await roleManager.CreateAsync(new IdentityRole<int>("User"));
        }
    }
}