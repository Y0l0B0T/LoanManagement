using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.LoansDefinition;

public class EFLoanDefinitionRepository(EfDataContext context) : LoanDefinitionRepository
{
    public void Add(LoanDefinition loanDefinition)
    {
        context.Set<LoanDefinition>().Add(loanDefinition);
    }

    public LoanDefinition Find(int id)
    {
        return context.Set<LoanDefinition>().FirstOrDefault(_ => _.Id == id);
    }
}