using LoanManagement.Persistence.Ef;
using LoanManagement.Persistence.Ef.Admins;
using LoanManagement.Persistence.Ef.LoansDefinition;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.LoansDefinition;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

namespace LoanManagement.TestTools.LoansDefinition;

public static class LoanDefinitionServiceFactory
{
    public static LoanDefinitionAppService Generate(EfDataContext context)
    {
        var adminRepository = new EFAdminRepository(context);
        var loanDefinitionRepository = new EFLoanDefinitionRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new LoanDefinitionAppService(adminRepository, loanDefinitionRepository, unitOfWork);
    }
}