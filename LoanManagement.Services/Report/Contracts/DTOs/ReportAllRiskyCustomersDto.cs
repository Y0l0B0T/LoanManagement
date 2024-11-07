namespace LoanManagement.Services.Report.Contracts.DTOs;

public class ReportAllRiskyCustomersDto
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string NationalCode { get; set; }
    public int DelayedInstallmentCount  { get; set; }
}