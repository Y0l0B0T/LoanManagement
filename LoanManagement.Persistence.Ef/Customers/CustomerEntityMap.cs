using LoanManagement.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanManagement.Persistence.Ef.Customers;

public class CustomerEntityMap : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).UseIdentityColumn();
        builder.Property(x => x.FirstName).HasMaxLength(30).IsRequired();
        builder.Property(x => x.LastName).HasMaxLength(30).IsRequired();
        builder.Property(x => x.PhoneNumber).HasMaxLength(11).IsRequired();
        builder.Property(x => x.NationalCode).HasMaxLength(10).IsRequired();
        builder.Property(x => x.Email).IsRequired(false);
        builder.Property(x => x.IsVerified).IsRequired();
        builder.Property(x => x.MonthlyIncome).HasColumnType("decimal(18,2)").IsRequired(false);
        builder.Property(x => x.JobType).IsRequired(false);
        builder.Property(x => x.Assets).HasColumnType("decimal(18,2)").IsRequired(false);
    }
}