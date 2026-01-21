using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;
using Project.Baw.Database;

namespace Project.Baw.Extensions;

public static class ServiceProviderExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddDatabase(IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDbContext>(opt =>
            {
                opt.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                opt.UseOpenIddict();
            });

            return services;
        }

        public IServiceCollection AddOpenIddictAuth(IConfiguration configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            var origins = configuration.GetSection("Origins").Get<string[]>() ??
                                throw new InvalidOperationException("No origins are configured in application settings.");
            
            services.AddCors(options
                => options.AddPolicy("AllowSpecificOrigin",
                    policy => policy
                        .WithOrigins(origins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            services.AddAuthorizationBuilder().AddPolicy("DefaultPolicy",
                policy =>
                {
                    policy.AuthenticationSchemes = [OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme];
                    policy.RequireAuthenticatedUser();
                });
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                        .UseDbContext<ApplicationDbContext>();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("connect/token");

                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();

                    options.AcceptAnonymousClients();

                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(60));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(30));

                    options.AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore()
                        .EnableTokenEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

            return services;
        }
    }
}