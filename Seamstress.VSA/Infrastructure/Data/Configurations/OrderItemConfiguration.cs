using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Quantity)
            .IsRequired();

        builder.Property(e => e.Size)
            .HasMaxLength(8)
            .IsRequired();

        builder.Property(e => e.OrderId)
            .IsRequired();
    }
}
