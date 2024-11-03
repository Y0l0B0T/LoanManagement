using FluentMigrator;

namespace LoanManagement.Migration;
[Migration(202411021815)]
public class _202411021815_AddCustomersTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Customers")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("FirstName").AsString(30).NotNullable()
            .WithColumn("LastName").AsString(30).NotNullable()
            .WithColumn("PhoneNumber").AsString(11).NotNullable()
            .WithColumn("NationalCode").AsString(10).NotNullable()
            .WithColumn("Email").AsString().Nullable()
            .WithColumn("Documents").AsString().Nullable()
            .WithColumn("IsVerified").AsBoolean().NotNullable()
            .WithColumn("MonthlyIncome").AsDecimal(18,2).Nullable()
            .WithColumn("JobType").AsInt32().Nullable()
            .WithColumn("Assets").AsDecimal(18,2).Nullable();
    }

    public override void Down()
    {
        Delete.Table("Customers");
    }
}