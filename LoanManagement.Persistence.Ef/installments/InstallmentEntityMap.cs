using LoanManagement.Entities.installments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Persistence.Ef.installments;

public class InstallmentEntityMap : IEntityTypeConfiguration<Installment>
{
    public void Configure(EntityTypeBuilder<Installment> builder)
    {
        builder.ToTable("Installments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.DueTime).IsRequired();
        builder.Property(x => x.PaymentTime).IsRequired(false);
        builder.Property(x => x.Status).IsRequired();
        builder.HasOne(x => x.Loan)
            .WithMany(x => x.Installments).HasForeignKey(x => x.LoanId).IsRequired();
    }
}