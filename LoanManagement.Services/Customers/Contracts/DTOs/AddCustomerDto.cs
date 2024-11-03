namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class AddCustomerDto
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string PhoneNumber { get; set; }
    public required string NationalCode { get; set; }
    public string? Email { get; set; }
}