using LoanManagement.Services.Report.Contracts.DTOs;

namespace LoanManagement.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController(ReportQuery reportQuery) : ControllerBase
{
    [HttpGet]
    public HashSet<ReportActiveLoanDto> ReportActiveLoan()
    {
        return reportQuery.ReportActiveLoan();
    }

    [HttpGet]
    public HashSet<ReportAllRiskyCustomersDto> ReportAllRiskyCustomers()
    {
        return reportQuery.ReportAllRiskyCustomers();
    }

    [HttpGet("{date}")]
    public ReportMonthlyIncomeDto ReportMonthlyIncome([FromRoute] DateOnly date)
    {
        return reportQuery.ReportMonthlyIncome(date);
    }

    [HttpGet]
    public HashSet<ReportAllClosedLoanDto> ReportAllClosedLoan()
    {
        return reportQuery.ReportAllClosedLoan();
    }
}