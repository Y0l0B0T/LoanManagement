using LoanManagement.Services.Loans.Contracts.DTOs;

namespace LoanManagement.Services.Loans.Contracts.Interfaces;

public interface LoanService
{
    void Add(AddLoanDto dto);
    void ConfirmLoan(int adminId, int loanId);
    void RejectLoan(int adminId, int loanId);
    void PayLoan(int adminId, int loanId);
    void DelayInPayLoan(int adminId, int loanId);
    void ClosedLoan(int adminId, int loanId);
}