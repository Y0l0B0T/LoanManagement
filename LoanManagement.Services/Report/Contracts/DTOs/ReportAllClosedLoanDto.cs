namespace LoanManagement.Services.Report.Contracts.DTOs;

public class ReportAllClosedLoanDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public int LoanDefinitionId { get; set; }
    public string? LoanType { get; set; }
    public decimal LoanAmount { get; set; }
    public int InstallmentsCount { get; set; }
    public decimal TotalPenaltyAmount { get; set; }
}