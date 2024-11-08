using System.ComponentModel.DataAnnotations;
using LoanManagement.Entities.Customers;

namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class AddFinancialInfoDto
{
    [Range(1, double.MaxValue)]public required decimal MonthlyIncome { get; set; }
    public required JobType JobType { get; set; }
    [Range(1, double.MaxValue)]public required decimal Assets { get; set; }
}