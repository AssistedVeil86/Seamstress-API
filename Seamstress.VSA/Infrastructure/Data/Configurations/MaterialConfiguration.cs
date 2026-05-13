using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(m => m.Detail)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(m => m.Quantity)
            .IsRequired();

        builder.Property(m => m.OrderId)
            .IsRequired();
    }
}
