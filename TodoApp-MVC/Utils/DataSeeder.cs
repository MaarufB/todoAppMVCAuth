using Microsoft.AspNetCore.Identity;
using TodoApp_MVC.Data;
using TodoApp_MVC.Models;

namespace TodoApp_MVC.Utils
{
    public class DataSeeder
    {
        public static async Task SeedData(IApplicationBuilder appBuilder)
        {
            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));


                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "maarufb@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var createAdminUser = new AppUser
                    {
                        UserName = "maarufb@gmail.com",
                        Email = adminUserEmail,
                        EmailConfirmed = true,

                    };

                    await userManager.CreateAsync(createAdminUser, "@Password1231");

                    AppUser user = await userManager.FindByEmailAsync(adminUserEmail);
                    if (user != null)
                        await userManager.AddToRoleAsync(createAdminUser, UserRoles.Admin);
                }


                // check admin if exist

                var checkAdmin = await userManager.FindByEmailAsync(adminUserEmail);
                if (checkAdmin != null)
                {
                    Console.WriteLine($"Email is not Null");
                }
                else
                {
                    Console.WriteLine($"Email Found with the user");
                }
            }
        }
    }
}
