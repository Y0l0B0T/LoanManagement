using LoanManagement.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Persistence.Ef.Loans;

public class LoanEntityMap : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("Loans");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.CreationDate).IsRequired();
        builder.Property(x => x.ValidationScore).IsRequired();
        builder.Property(x => x.Status).HasColumnType("int").IsRequired();
        builder.Property(x => x.LoanType).IsRequired();
        
        builder.HasOne(x => x.Customer).WithMany()
            .HasForeignKey(x => x.CustomerId).IsRequired(false);
        
        builder.HasOne(x => x.LoanDefinition).WithMany()
            .HasForeignKey(x => x.LoanDefinitionId).IsRequired();
        
        builder.Property(x => x.Status).HasConversion<int>();
    }
}