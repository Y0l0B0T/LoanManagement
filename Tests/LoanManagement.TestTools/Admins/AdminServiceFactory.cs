using LoanManagement.Persistence.Ef;
using LoanManagement.Persistence.Ef.Admins;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.Admins;

namespace LoanManagement.TestTools.Admins;

public static class AdminServiceFactory
{
    public static AdminAppService Generate(EfDataContext context)
    {
        
        var adminRepository = new EFAdminRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new AdminAppService(adminRepository, unitOfWork);
    }
}