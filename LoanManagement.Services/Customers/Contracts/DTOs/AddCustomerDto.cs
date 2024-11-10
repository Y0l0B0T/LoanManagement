namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class AddCustomerDto
{
    [MinLength(2)]
    public required string FirstName { get; set; }
    [MinLength(2)]
    public required string LastName { get; set; }
    [RegularExpression(@"^0[0-9]{10}$")]public required string PhoneNumber { get; set; }
    [StringLength(10, MinimumLength = 10)]public required string NationalCode { get; set; }
    public string? Email { get; set; }
}