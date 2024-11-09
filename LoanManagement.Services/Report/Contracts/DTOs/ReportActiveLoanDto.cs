namespace LoanManagement.Services.Report.Contracts.DTOs;

public class ReportActiveLoanDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? LoanType { get; set; }
    public LoanStatus Status { get; set; }
    public decimal PaymentAmount { get; set; }
    public HashSet<GetPendingInstallmentsByLoanIdDto> PendingInstallments { get; set; } = [];
}