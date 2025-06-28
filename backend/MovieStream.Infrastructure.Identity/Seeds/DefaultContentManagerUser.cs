using Microsoft.AspNetCore.Identity;
using MovieStream.Core.Application.Enums;
using MovieStream.Infrastructure.Identity.Entities;

namespace MovieStream.Infrastructure.Identity.Seeds
{
    public static class DefaultContentManagerUser
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            AppUser defaultUser = new();
            defaultUser.UserName = "contentmanageruser";
            defaultUser.Email = "contentmanageruser@email.com";
            defaultUser.FirstName = "John";
            defaultUser.LastName = "Doe";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, nameof(Roles.ContentManager));
                }
            }
        }
    }
}
