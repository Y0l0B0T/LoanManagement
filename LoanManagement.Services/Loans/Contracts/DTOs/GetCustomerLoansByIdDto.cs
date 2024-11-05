using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;

namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class GetCustomerLoansByIdDto
{
    public string? LoanType { get; set; }
    public LoanStatus Status { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];
}