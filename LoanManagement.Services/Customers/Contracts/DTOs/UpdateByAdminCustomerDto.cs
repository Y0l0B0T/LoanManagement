namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class UpdateByAdminCustomerDto
{
    [MinLength(2)] public required string FirstName { get; set; }
    [MinLength(2)] public required string LastName { get; set; }
    [RegularExpression(@"^0[0-9]{10}$")]public required string PhoneNumber { get; set; }
    [StringLength(10, MinimumLength = 10)]public required string NationalCode { get; set; }
    public string? Email { get; set; }
    [MinLength(2)] public string? Documents { get; set; }
    public bool IsVerified { get; set; }
    [Range(1, double.MaxValue)]public decimal? MonthlyIncome { get; set; }
    [Range(1, 3)] public JobType? JobType { get; set; }
    [Range(1, double.MaxValue)]public decimal? Assets { get; set; }
}