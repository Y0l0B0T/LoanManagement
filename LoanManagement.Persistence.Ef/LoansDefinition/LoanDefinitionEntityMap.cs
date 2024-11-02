using LoanManagement.Entities.LoansDefinition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Persistence.Ef.LoansDefinition;

public class LoanDefinitionEntityMap : IEntityTypeConfiguration<LoanDefinition>
{
    public void Configure(EntityTypeBuilder<LoanDefinition> builder)
    {
        builder.ToTable("LoansDefinition");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.LoanAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.InterestRate).HasColumnType("decimal(5,2)").IsRequired();
        builder.Property(x => x.InstallmentsCount).IsRequired();
        builder.Property(x => x.InstallmentAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.MonthlyInterestAmount).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(x => x.MonthlyPenaltyAmount).HasColumnType("decimal(18,2)").IsRequired();
    }
}