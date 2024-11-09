namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class GetLoansByNationalCodeDto
{
    public LoanStatus Status { get; set; }
    public string? LoanType { get; set; }
    public int InstallmentsCount { get; set; }
}