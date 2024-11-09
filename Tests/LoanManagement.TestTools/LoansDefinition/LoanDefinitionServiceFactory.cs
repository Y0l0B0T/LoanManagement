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