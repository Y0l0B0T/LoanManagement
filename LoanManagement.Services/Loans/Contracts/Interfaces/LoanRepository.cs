using LoanManagement.Entities.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;

namespace LoanManagement.Services.Loans.Contracts.Interfaces;

public interface LoanRepository
{
    void Add(Loan loan);
    bool IsDuplicate(int customerId, int loanDefinitionId);
    HashSet<GetCustomerLoansByIdDto> GetCustomerLoansById(int customerId);
    Loan Find(int loanId);
    void Update(Loan loan);
}