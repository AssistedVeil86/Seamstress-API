using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Seamstress.VSA.Domain.Entities;
using Seamstress.VSA.Infrastructure.Data.Configurations;

namespace Seamstress.VSA.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<Detail> Details { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<WeeklyReport> WeeklyReports { get; set; }
    public DbSet<Expense> Expenses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureIdentitySchema();
        modelBuilder.HasDefaultSchema("contSys");
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
