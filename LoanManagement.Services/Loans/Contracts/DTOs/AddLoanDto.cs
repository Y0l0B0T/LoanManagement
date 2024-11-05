using LoanManagement.Entities.Loans;

namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class AddLoanDto
{
    public int CustomerId { get; set; }
    public int LoanDefinitionId { get; set; }
}