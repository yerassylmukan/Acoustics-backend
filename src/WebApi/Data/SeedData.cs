using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Domain;

namespace WebApi.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext dbContext, UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        if (dbContext.Database.IsNpgsql()) dbContext.Database.Migrate();

        await EnsureRoleExistsAsync(roleManager, "Admin");
        await EnsureRoleExistsAsync(roleManager, "BasicUser");
        await EnsureRoleExistsAsync(roleManager, "Anonymous");

        var adminUserName = "admin@gmail.com";
        var adminUser = new ApplicationUser
        {
            UserName = adminUserName,
            Email = adminUserName
        };

        if (await userManager.FindByEmailAsync(adminUserName) == null)
        {
            await userManager.CreateAsync(adminUser, "Admin@24323skfnskn");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var userUserName = "user@gmail.com";
        var user = new ApplicationUser
        {
            UserName = userUserName,
            Email = userUserName
        };

        if (await userManager.FindByEmailAsync(userUserName) == null)
        {
            await userManager.CreateAsync(user, "User@24323skfnskn");
            await userManager.AddToRoleAsync(user, "BasicUser");
        }
    }

    private static async Task EnsureRoleExistsAsync(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName)) await roleManager.CreateAsync(new IdentityRole(roleName));
    }
}