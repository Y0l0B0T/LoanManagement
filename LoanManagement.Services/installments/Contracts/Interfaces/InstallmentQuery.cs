namespace LoanManagement.Services.installments.Contracts.Interfaces;

public interface InstallmentQuery
{
    Installment? GetById(int installmentId);
    HashSet<GetAllInstallmentsDto> GetAll();
    HashSet<GetAllInstallmentsOfLoanDto> GetAllInstallmentsOfLoan(int loanId);
}