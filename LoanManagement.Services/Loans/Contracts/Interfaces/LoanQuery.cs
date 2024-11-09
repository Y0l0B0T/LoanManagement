namespace LoanManagement.Services.Loans.Contracts.Interfaces;

public interface LoanQuery
{
    Loan? GetById(int loanId);
    HashSet<GetAllLoansDto> GetAll();
    HashSet<GetAllLoansDto> GetAllInReview();
    HashSet<GetAllLoansDto> GetAllInApproved();
    HashSet<GetAllLoansDto> GetAllInRejected();
    HashSet<GetAllLoansDto> GetAllInPaying();
    HashSet<GetAllLoansDto> GetAllActiveLoans();
    HashSet<GetAllLoansDto> GetAllInClosed();
    HashSet<GetPendingInstallmentsByLoanIdDto> GetPendingInstallmentsByLoanId(int loadId);
}