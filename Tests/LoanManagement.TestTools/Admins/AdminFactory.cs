using LoanManagement.Entities.Admins;

namespace LoanManagement.TestTools.Admins;

public static class AdminFactory
{
    public static Admin Create(string adminName = "Admin")
    {
        return new Admin()
        {
            Name = adminName
        };
    }
}