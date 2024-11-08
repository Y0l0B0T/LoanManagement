using System.ComponentModel.DataAnnotations;
using LoanManagement.Entities.Customers;

namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class UpdateCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    [RegularExpression(@"^0[0-9]{10}$")]public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    [Range(1, double.MaxValue)]public decimal? MonthlyIncome { get; set; }
    public JobType? JobType { get; set; }
    [Range(1, double.MaxValue)]public decimal? Assets { get; set; }
}