using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public static class IdentityConfiguration
{
    public static void ConfigureIdentitySchema(this ModelBuilder modelBuilder, string schema = "identity")
    {
        modelBuilder.Entity<IdentityUser>().ToTable("Users", schema);
        modelBuilder.Entity<IdentityRole>().ToTable("Roles", schema);
        modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", schema);
        modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", schema);
        modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", schema);
        modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", schema);
        modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", schema);
    }
}
