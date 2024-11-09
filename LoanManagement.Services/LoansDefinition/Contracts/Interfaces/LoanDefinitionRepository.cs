namespace LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

public interface LoanDefinitionRepository
{
    void Add(LoanDefinition loanDefinition);
    LoanDefinition Find(int id);
}