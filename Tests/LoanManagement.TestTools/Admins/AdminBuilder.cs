using LoanManagement.Entities.Admins;

namespace LoanManagement.TestTools.Admins;

public class AdminBuilder
{
    private readonly Admin _admin;
    public AdminBuilder()
    {
        _admin = new Admin()
        {
            Name = "Admin"
        };
    }
    public AdminBuilder WithName(string name)
    {
        _admin.Name = name;
        return this;
    }

    public Admin Build()
    {
        return _admin;
    }
}