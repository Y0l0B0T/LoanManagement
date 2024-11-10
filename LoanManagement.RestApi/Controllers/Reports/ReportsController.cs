using LoanManagement.Services.Report.Contracts.DTOs;

namespace LoanManagement.RestApi.Controllers.Reports;

[ApiController]
[Route("api/v1/reports")]
public class ReportsController(ReportQuery reportQuery) : ControllerBase
{
    [HttpGet("activeloan")]
    public HashSet<ReportActiveLoanDto> ReportActiveLoan()
    {
        return reportQuery.ReportActiveLoan();
    }

    [HttpGet("riskycustomers")]
    public HashSet<ReportAllRiskyCustomersDto> ReportAllRiskyCustomers()
    {
        return reportQuery.ReportAllRiskyCustomers();
    }

    [HttpGet("{date}/monthlyincome")]
    public ReportMonthlyIncomeDto ReportMonthlyIncome(DateOnly date)
    {
        return reportQuery.ReportMonthlyIncome(date);
    }

    [HttpGet("closedloans")]
    public HashSet<ReportAllClosedLoanDto> ReportAllClosedLoan()
    {
        return reportQuery.ReportAllClosedLoan();
    }
}