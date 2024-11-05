using FluentMigrator;

namespace LoanManagement.Migration;
[Migration(202411021817)]
public class _202411021817_AddLoansDefinitionTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("LoansDefinition")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(30).NotNullable()
            .WithColumn("LoanAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("InterestRate").AsDecimal().NotNullable()
            .WithColumn("InstallmentsCount").AsInt32().NotNullable()
            .WithColumn("BasePaymentAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("InstallmentAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("MonthlyInterestAmount").AsDecimal(18, 2).NotNullable()
            .WithColumn("MonthlyPenaltyAmount").AsDecimal(18, 2).Nullable();
    }

    public override void Down()
    {
        Delete.Table("LoansDefinition");
    }
}