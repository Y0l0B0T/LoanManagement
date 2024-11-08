using LoanManagement.Entities.installments;

namespace LoanManagement.Services.installments.Contracts.DTOs;

public class GetAllInstallmentsOfLoanDto
{
    public int Id { get; set; }
    public DateOnly DueTime { get; set; }
    public DateOnly? PaymentTime { get; set; }
    public InstallmentStatus Status { get; set; }
}