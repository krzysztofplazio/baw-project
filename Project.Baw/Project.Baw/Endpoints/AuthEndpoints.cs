using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace Project.Baw.Endpoints;

public static class AithEndpoints
{
    public static WebApplication MapOpenIddictEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/")
            .WithTags("Auth");

        root.MapPost("/connect/token", Token);

        return app;
    }

    private static async Task<IResult> Token(
        HttpContext httpContext,
        UserManager<IdentityUser> userManager)
    {
        var request = httpContext.GetOpenIddictServerRequest()
                      ?? throw new InvalidOperationException("OpenIddict request is missing.");

        if (!request.IsPasswordGrantType())
        {
            return Results.BadRequest(new
            {
                error = "unsupported_grant_type"
            });
        }

        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Results.BadRequest(new
            {
                error = "invalid_grant",
                error_description = "Invalid username or password."
            });
        }

        var identity = new ClaimsIdentity(
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id);
        identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email ?? "");
        identity.AddClaim(OpenIddictConstants.Claims.Name, user.UserName ?? "");

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(request.GetScopes());
        principal.SetResources("api");

        return Results.SignIn(principal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }
}