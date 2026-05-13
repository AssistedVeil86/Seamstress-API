using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public class DetailConfiguration : IEntityTypeConfiguration<Detail>
{
    public void Configure(EntityTypeBuilder<Detail> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(d => d.OrderId)
            .IsRequired();
    }
}
