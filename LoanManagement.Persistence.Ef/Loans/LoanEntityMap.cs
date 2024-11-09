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
        builder.Property(x => x.Status).IsRequired();
        builder.Property(x => x.LoanType).IsRequired();
        
        builder.HasOne(x => x.Customer).WithMany(x=>x.MyLoans)
            .HasForeignKey(x => x.CustomerId).IsRequired();
        
        builder.HasOne(x => x.LoanDefinition).WithMany(x=>x.AllLoans)
            .HasForeignKey(x => x.LoanDefinitionId).IsRequired();
        
    }
}