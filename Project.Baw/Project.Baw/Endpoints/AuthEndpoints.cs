using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using Project.Baw.Database;
using Project.Baw.Database.Models;

namespace Project.Baw.Endpoints;

public static class AithEndpoints
{
    public static WebApplication MapOpenIddictEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/")
            .WithTags("Auth");

        root.MapPost("/connect/token", Token);
        root.MapPost("/connect/register", Register);

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

    private static async Task<IResult> Register(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        RegisterRequest request,
        ApplicationDbContext db)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password) ||
            string.IsNullOrWhiteSpace(request.Phone))
        {
            return Results.BadRequest(new { error = "invalid_request", error_description = "Username, password and phone are required." });
        }

        if (!await roleManager.RoleExistsAsync("User"))
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        var user = new IdentityUser
        {
            UserName = request.Username,
            Email = request.Username,
            PhoneNumber = request.Phone,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, request.Password);

        db.Clients.Add(new Client()
        {
            Id = Guid.NewGuid(),
            IdentityUserId = user.Id,
            Phone = request.Phone
        });
        await db.SaveChangesAsync();

        if (!result.Succeeded)
        {
            return Results.BadRequest(new
            {
                error = "invalid_request",
                errors = result.Errors.Select(x => x.Description)
            });
        }

        await userManager.AddToRoleAsync(user, "User");

        return Results.Created($"/users/{user.Id}", new { user.Id, user.UserName });
    }

    public sealed record RegisterRequest(string Username, string Password, string Phone);
}
