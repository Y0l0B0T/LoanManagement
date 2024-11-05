using LoanManagement.Entities.installments;

namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class GetPendingInstallmentsByLoanId
{
    public int Id { get; set; }
    public DateOnly DueTime { get; set; }
    public InstallmentStatus Status { get; set; }
}