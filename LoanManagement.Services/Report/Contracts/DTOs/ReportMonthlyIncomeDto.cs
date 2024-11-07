namespace LoanManagement.Services.Report.Contracts.DTOs;

public class ReportMonthlyIncomeDto
{
    public decimal TotalIncomeFromInterest { get; set; }
    public decimal TotalIncomeFromPenalty { get; set; }
}