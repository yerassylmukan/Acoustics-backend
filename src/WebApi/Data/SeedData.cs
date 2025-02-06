using Microsoft.EntityFrameworkCore;
using WebApi.Common.Contracts;
using WebApi.Domain;

namespace WebApi.Data;

public static class SeedData
{
    public static async Task SeedAsync(AppDbContext dbContext, IUserRepository userRepository)
    {
        if (dbContext.Database.IsNpgsql())
        {
            dbContext.Database.Migrate();

            var adminPhoneNumber = "77789065778";
            var admin = new User
            {
                Id = Guid.NewGuid(),
                Username = adminPhoneNumber,
                PhoneNumber = adminPhoneNumber,
                Name = "Admin",
                PictuireUrl = "",
                CreationDate = DateTime.Now,
                Role = "Admin"
            };

            if (await userRepository.GetUserByUsername(admin.Username) == null)
            {
                dbContext.Users.Add(admin);
                await dbContext.SaveChangesAsync();
            }

            var userPhoneNumber = "77764194585";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userPhoneNumber,
                PhoneNumber = userPhoneNumber,
                Name = "Krutoi User",
                PictuireUrl = "",
                CreationDate = DateTime.Now,
                Role = "User"
            };

            if (await userRepository.GetUserByUsername(user.Username) == null)
            {
                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}