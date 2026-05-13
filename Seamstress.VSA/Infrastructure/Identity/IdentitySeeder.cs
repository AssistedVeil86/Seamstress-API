using Microsoft.AspNetCore.Identity;

namespace Seamstress.VSA.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        string[] roles = ["Admin", "User"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        var adminEmail = configuration["IdentityUsers:AdminEmail"]
            ?? throw new ArgumentNullException("Falta el email del administrador en la configuración.");

        var adminPassword = configuration["IdentityUsers:AdminPwd"]
            ?? throw new ArgumentNullException("Falta el password del administrador en la configuración.");

        var ownerEmail = configuration["IdentityUsers:OwnerEmail"]
            ?? throw new ArgumentNullException("Falta el email del Dueño en la configuración.");

        var ownerPassword = configuration["IdentityUsers:OwnerPwd"]
            ?? throw new ArgumentNullException("Falta el password del administrador en la configuración.");

        var users = new Dictionary<string, string>
        {
            { adminEmail, adminPassword },
            { ownerEmail, ownerPassword }
        };

        foreach(KeyValuePair<string, string> user in users)
        {
            if (await userManager.FindByEmailAsync(user.Key) == null)
            {
                var identityuser = new IdentityUser
                {
                    UserName = user.Key,
                    Email = user.Key,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(identityuser, user.Value);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(identityuser, "Admin");
                }
            }
        }
    }
}
