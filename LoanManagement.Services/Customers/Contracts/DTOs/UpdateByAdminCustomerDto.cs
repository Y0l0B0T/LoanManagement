using LoanManagement.Entities.Customers;

namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class UpdateByAdminCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public string? Email { get; set; }
    public string? Documents { get; set; }
    public bool IsVerified { get; set; }
    public decimal? MonthlyIncome { get; set; }
    public JobType? JobType { get; set; }
    public decimal? Assets { get; set; }
}