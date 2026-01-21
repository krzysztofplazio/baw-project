using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace Project.Baw.Helpers;

public static class OpenIddictHelpers
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        var manager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        const string clientId = "local-client";

        var existing = await manager.FindByClientIdAsync(clientId);

        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = clientId,
            DisplayName = "Local Password Client",
            Permissions =
            {
                OpenIddictConstants.Permissions.Endpoints.Token,
                OpenIddictConstants.Permissions.GrantTypes.Password,
                OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Prefixes.Scope + "api"
            }
        };

        if (existing == null)
        {
            await manager.CreateAsync(descriptor);
        }
        else
        {
            await manager.UpdateAsync(existing, descriptor);
        }

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