using Microsoft.AspNetCore.Identity;
using TicketManagement.Core.Public.Enums;
using TicketManagement.DataAccess.EF.Core.Entities;

namespace TicketManagement.UserManagement.API.Helpers
{
    internal static class IdentitySeed
    {
        public static async Task SeedAsync(UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            const string adminEmail = "admin@admin.com";
            const string moderatorEmail = "moderator@moderator.com";
            const string userEmail = "user@user.com";

            const string password = "_Aa12345";

            await SeedRolesAsync(roleManager);

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var user = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TimeZoneId = "UTC",
                    Language = "en-US",
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
            }

            if (await userManager.FindByNameAsync(moderatorEmail) == null)
            {
                var user = new User
                {
                    Email = moderatorEmail,
                    UserName = moderatorEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TimeZoneId = "UTC",
                    Language = "be-BY",
                };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Moderator.ToString());
                }
            }

            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                var user = new User
                {
                    Email = userEmail,
                    UserName = userEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    TimeZoneId = "UTC",
                    Language = "ru-RU",
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
                }
            }
        }

        private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
        {
            if (await roleManager.FindByNameAsync(Roles.Admin.ToString()) == null)
            {
                await roleManager.CreateAsync(new Role(Roles.Admin.ToString()));
            }

            if (await roleManager.FindByNameAsync(Roles.User.ToString()) == null)
            {
                await roleManager.CreateAsync(new Role(Roles.User.ToString()));
            }

            if (await roleManager.FindByNameAsync(Roles.PremiumUser.ToString()) == null)
            {
                await roleManager.CreateAsync(new Role(Roles.PremiumUser.ToString()));
            }

            if (await roleManager.FindByNameAsync(Roles.Moderator.ToString()) == null)
            {
                await roleManager.CreateAsync(new Role(Roles.Moderator.ToString()));
            }
        }
    }
}