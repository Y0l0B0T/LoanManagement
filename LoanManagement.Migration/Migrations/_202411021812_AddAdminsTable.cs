using FluentMigrator;

namespace LoanManagement.Migration;
[Migration(202411021812)]
public class _202411021812_AddAdminsTable : FluentMigrator.Migration
{
    public override void Up()
    {
        Create.Table("Admins")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Name").AsString(30).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("Admins");
    }
}