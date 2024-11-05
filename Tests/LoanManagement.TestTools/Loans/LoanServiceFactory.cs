using LoanManagement.Persistence.Ef;
using LoanManagement.Persistence.Ef.Admins;
using LoanManagement.Persistence.Ef.Customers;
using LoanManagement.Persistence.Ef.Loans;
using LoanManagement.Persistence.Ef.LoansDefinition;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.Loans;

namespace LoanManagement.TestTools.Loans;

public static class LoanServiceFactory
{
    public static LoanAppService Generate(EfDataContext context)
    {
        var adminRepository = new EFAdminRepository(context);
        var customerRepository = new EFCustomerRepository(context);
        var loanRepository = new EFLoanRepository(context);
        var loanDefinition = new EFLoanDefinitionRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new LoanAppService(adminRepository, customerRepository, loanRepository, loanDefinition, unitOfWork);
    }
}