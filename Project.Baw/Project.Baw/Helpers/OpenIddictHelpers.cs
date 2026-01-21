using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Project.Baw.Helpers;

public static class OpenIddictHelpers
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        var user = await userManager.FindByNameAsync("admin@baw.pl");
        if (user != null)
        {
            return;
        }

        user = new IdentityUser
        {
            UserName = "admin@baw.pl",
            Email = "admin@baw.pl",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, "Admin123!");
        if (!result.Succeeded)
            throw new Exception(string.Join(", ", result.Errors.Select(x => x.Description)));

        await userManager.AddToRoleAsync(user, "Admin");
    }
}