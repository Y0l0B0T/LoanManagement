using LoanManagement.Entities.installments;
using LoanManagement.Services.installments.Contracts.DTOs;

namespace LoanManagement.Services.installments.Contracts.Interfaces;

public interface InstallmentQuery
{
    Installment? GetById(int installmentId);
    HashSet<GetAllInstallmentsDto> GetAll();
    HashSet<GetallInstallmentsOfLoanDto> GetAllInstallmentsOfLoan(int loanId);
}