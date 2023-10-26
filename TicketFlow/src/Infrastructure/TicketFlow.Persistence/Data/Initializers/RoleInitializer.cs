using Microsoft.AspNetCore.Identity;
using TicketFlow.Domain.Enums.Users;

namespace TicketFlow.Persistence.Data.Initializers;

public class RoleInitializer
{
    public static async Task RoleInitializeAsync(RoleManager<IdentityRole<string>> roleManager)
    {
        if (await roleManager.FindByNameAsync(UserRole.Admin.ToString()) == null)
        {
            var adminRole = new IdentityRole<string>
            {
                Id = Guid.NewGuid().ToString(),  
                Name = UserRole.Admin.ToString()
            };
            await roleManager.CreateAsync(adminRole);
        }

        if (await roleManager.FindByNameAsync(UserRole.User.ToString()) == null)
        {
            var userRole = new IdentityRole<string>
            {
                Id = Guid.NewGuid().ToString(),  
                Name = UserRole.User.ToString()            
            };
            await roleManager.CreateAsync(userRole);
        }
        
        if (await roleManager.FindByNameAsync(UserRole.Owner.ToString()) == null)
        {
            var userRole = new IdentityRole<string>
            {
                Id = Guid.NewGuid().ToString(),  
                Name = UserRole.Owner.ToString()
            };
            await roleManager.CreateAsync(userRole);
        }
    }
}