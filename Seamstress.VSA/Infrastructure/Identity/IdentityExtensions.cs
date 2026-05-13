using System;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Seamstress.VSA.Infrastructure.Data;

namespace Seamstress.VSA.Infrastructure.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<BearerTokenOptions>(IdentityConstants.BearerScheme,
                options =>
                {
                    options.BearerTokenExpiration = TimeSpan.FromHours(8);
                    options.RefreshTokenExpiration = TimeSpan.FromDays(30);
                });

        return services;
    }
}
