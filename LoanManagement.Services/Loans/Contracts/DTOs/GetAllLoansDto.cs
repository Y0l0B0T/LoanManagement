namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class GetAllLoansDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanDefinitionId { get; set; }
    public DateOnly CreationDate { get; set; }
    public int ValidationScore { get; set; }
    public LoanStatus Status { get; set; }
    public string? LoanType { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];
}