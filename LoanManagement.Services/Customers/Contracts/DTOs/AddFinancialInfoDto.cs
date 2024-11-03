using LoanManagement.Entities.Customers;

namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class AddFinancialInfoDto
{
    public required decimal MonthlyIncome { get; set; }
    public required JobType JobType { get; set; }
    public required decimal Assets { get; set; }
}