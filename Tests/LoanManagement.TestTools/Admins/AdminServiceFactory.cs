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