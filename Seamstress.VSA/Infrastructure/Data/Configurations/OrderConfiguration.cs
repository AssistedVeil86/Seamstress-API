using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(o => o.Name)
            .IsRequired();

        builder.Property(e => e.Total)
            .HasColumnType("DECIMAL(18, 2)");

        builder.Property(e => e.Status)
            .HasConversion<string>();

        builder.HasMany(o => o.Materials)
            .WithOne(m => m.Order)
            .HasForeignKey(m => m.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Details)
            .WithOne(d => d.Order)
            .HasForeignKey(d => d.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}