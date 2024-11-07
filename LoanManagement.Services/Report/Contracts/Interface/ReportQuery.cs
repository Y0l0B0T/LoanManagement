using LoanManagement.Services.Report.Contracts.DTOs;

namespace LoanManagement.Services.Report.Contracts.Interface;

public interface ReportQuery
{
    HashSet<ReportActiveLoanDto> ReportActiveLoan();
    HashSet<ReportAllRiskyCustomersDto> ReportAllRiskyCustomers();
    HashSet<ReportMonthlyIncomeDto> ReportMonthlyIncome(DateOnly date);
    HashSet<ReportAllClosedLoanDto> ReportAllClosedLoan();
}