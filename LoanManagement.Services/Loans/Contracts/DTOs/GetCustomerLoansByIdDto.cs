namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class GetCustomerLoansByIdDto
{
    public string? LoanType { get; set; }
    public LoanStatus Status { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];
}