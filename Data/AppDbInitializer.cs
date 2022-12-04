using eMobileShop.Helper;
using eMobileShop.Models;
using Microsoft.AspNetCore.Identity;

namespace eMobileShop.Data;

public class AppDbInitializer
{
    public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
    {
        using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            //Roles
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(AppConstants.ADMIN))
                await roleManager.CreateAsync(new IdentityRole(AppConstants.ADMIN));

            if (!await roleManager.RoleExistsAsync(AppConstants.USER))
                await roleManager.CreateAsync(new IdentityRole(AppConstants.USER));

            //Users
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var adminEmail = "admin@admin.com";

            var admin = await userManager.FindByEmailAsync(adminEmail);
            if(admin == null)
            {
                var adminUser = new ApplicationUser()
                {
                    FullName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    UserName = "Admin-user",
                    Address = "India"
                };

                await userManager.CreateAsync(adminUser, "Test@123");
                await userManager.AddToRoleAsync(adminUser, AppConstants.ADMIN);
            }

            var userEmail = "user@user.com";

            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                var newUser = new ApplicationUser()
                {
                    FullName = "User",
                    Email = userEmail,
                    EmailConfirmed = true,
                    UserName = "Test-user",
                    Address = "India",
                    PhoneNumber = "1234567890"
                };

                await userManager.CreateAsync(newUser, "Test@123");
                await userManager.AddToRoleAsync(newUser, AppConstants.USER);
            }
        }
    }
}
