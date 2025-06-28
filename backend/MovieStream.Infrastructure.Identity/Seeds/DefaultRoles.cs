using Microsoft.AspNetCore.Identity;
using MovieStream.Core.Application.Enums;
using MovieStream.Infrastructure.Identity.Entities;

namespace MovieStream.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.Admin)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.ContentManager)));
            await roleManager.CreateAsync(new IdentityRole(nameof(Roles.User)));
        }
    }
}
