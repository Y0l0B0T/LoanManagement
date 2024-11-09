namespace LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

public interface LoanDefinitionQuery
{
    LoanDefinition? GetById(int loanDefinitionId);
    HashSet<GetAllLoanDefinitionDto> GetAll();
}