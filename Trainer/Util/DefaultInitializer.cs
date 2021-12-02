using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Trainer.DAL.Entities;

namespace Trainer.Util
{
    public class DefaultInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("doctor") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("doctor"));
            }
            if (await userManager.FindByNameAsync("admin@gmail.com") == null)
            {
                User admin = new User { Email = "admin@gmail.com", UserName = "admin"};
                IdentityResult result = await userManager.CreateAsync(admin, "admin");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
            if (await userManager.FindByNameAsync("doctor@gmail.com") == null)
            {
                User admin = new User { Email = "doctor@gmail.com", UserName = "doctor" };
                IdentityResult result = await userManager.CreateAsync(admin, "doctor");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "doctor");
                }
            }
        }
    }
}
