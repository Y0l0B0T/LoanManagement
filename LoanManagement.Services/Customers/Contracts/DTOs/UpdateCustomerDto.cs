using LoanManagement.Entities.Customers;

namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class UpdateCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public string? Email { get; set; }
    public decimal? MonthlyIncome { get; set; }
    public JobType? JobType { get; set; }
    public decimal? Assets { get; set; }
}