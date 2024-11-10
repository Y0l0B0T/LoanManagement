namespace LoanManagement.Services.Customers.Contracts.DTOs;

public class AddDocumentsDto
{
    [MinLength(1)]
    public required string Documents { get; set; }
}