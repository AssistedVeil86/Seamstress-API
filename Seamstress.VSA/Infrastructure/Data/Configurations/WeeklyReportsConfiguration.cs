using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Seamstress.VSA.Domain.Entities;

namespace Seamstress.VSA.Infrastructure.Data.Configurations;

public class WeeklyReportsConfiguration : IEntityTypeConfiguration<WeeklyReport>
{
    public void Configure(EntityTypeBuilder<WeeklyReport> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.TotalIncome)
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(e => e.Isr)
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(e => e.TotalEmployeesExpense)
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(e => e.TotalSuppliesExpense)
            .HasColumnType("DECIMAL(18,2)");

        builder.Property(e => e.NetProfit)
            .HasColumnType("DECIMAL(18,2)");
    }
}
