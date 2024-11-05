using LoanManagement.Services.installments.Contracts.DTOs;

namespace LoanManagement.Services.installments.Contracts.Interfaces;

public interface InstallmentService
{
    void PayInstallment(int installmentId,PayInstallmentDto dto);
}