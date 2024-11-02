using FluentMigrator;

namespace LoanManagement.Migration;
[Migration(202411021827)]
public class _202411021827_AddInstallmentsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Installments")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("DueTime").AsDate().NotNullable()
            .WithColumn("PaymentTime").AsDate().Nullable()
            .WithColumn("LoanId").AsInt32().Nullable()
            .ForeignKey("FK_Installments_Loans", "Loans", "Id");
    }

    public override void Down()
    {
        Delete.Table("Installments");
    }
}