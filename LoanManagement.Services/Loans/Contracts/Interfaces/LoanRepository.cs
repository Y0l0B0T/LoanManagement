namespace LoanManagement.Services.Loans.Contracts.Interfaces;

public interface LoanRepository
{
    void Add(Loan loan);
    bool IsDuplicate(int customerId, int loanDefinitionId);
    HashSet<GetCustomerLoansByIdDto> GetCustomerLoansById(int customerId);
    Loan Find(int loanId);
    Loan FindInstallments(int loanId);
    void Update(Loan loan);
    void AddInstallmens(Loan loan);
}