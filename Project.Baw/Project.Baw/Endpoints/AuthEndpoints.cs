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

        root.MapGet("/connect/authorize", Authorize);
        root.MapPost("/connect/token", (Delegate)Token);

        return app;
    }

    private static async Task<IResult> Authorize(
        HttpContext httpContext,
        UserManager<IdentityUser> userManager)
    {
        var request = httpContext.GetOpenIddictServerRequest()
                      ?? throw new InvalidOperationException("OpenIddict request is missing.");

        if (!httpContext.User.Identity?.IsAuthenticated ?? true)
        {
            return Results.Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = httpContext.Request.Path + httpContext.Request.QueryString
                },
                authenticationSchemes:
                [
                    IdentityConstants.ApplicationScheme
                ]);
        }

        var user = await userManager.GetUserAsync(httpContext.User)
                   ?? throw new InvalidOperationException("User not found.");

        var claims = new List<Claim>
        {
            new(OpenIddictConstants.Claims.Subject, user.Id),
            new(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty),
            new(OpenIddictConstants.Claims.Name, user.UserName ?? string.Empty)
        };

        var identity = new ClaimsIdentity(
            claims,
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        principal.SetScopes(request.GetScopes());
        principal.SetResources("api");

        return Results.SignIn(
            principal,
            authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    private static async Task<IResult> Token(
        HttpContext httpContext,
        UserManager<IdentityUser> userManager)
    {
        var request = httpContext.GetOpenIddictServerRequest()
                      ?? throw new InvalidOperationException("OpenIddict request is missing.");

        // -----------------------
        // Password grant
        // -----------------------
        if (request.IsPasswordGrantType())
        {
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

        return Results.BadRequest(new
        {
            error = "unsupported_grant_type"
        });
    }
}