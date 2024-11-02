using FluentMigrator;

namespace LoanManagement.Migration;
[Migration(202411021822)]
public class _202411021822_AddLoansTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Loans")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("CreationDate").AsDate().NotNullable()
            .WithColumn("ValidationScore").AsInt32().NotNullable()
            .WithColumn("Status").AsInt32().NotNullable()
            .WithColumn("LoanType").AsString().NotNullable()
            .WithColumn("CustomerId").AsInt32().Nullable()
            .ForeignKey("FK_Loans_Customers", "Customers", "Id")
            .WithColumn("LoanDefinitionId").AsInt32().NotNullable()
            .ForeignKey("FK_Loans_LoansDefinition", "LoansDefinition", "Id");
        
    }

    public override void Down()
    {
        Delete.Table("Loans");
    }
}