namespace LoanManagement.TestTools.Customers;

public static class CustomerServiceFactory
{
    public static CustomerAppService Generate(EfDataContext context)
    {
        var adminRepository = new EFAdminRepository(context);
        var customerRepository = new EFCustomerRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new CustomerAppService(adminRepository, customerRepository, unitOfWork);
    }
}