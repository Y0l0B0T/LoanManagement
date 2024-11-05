using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;

namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class GetCustomerLoansByIdDto
{
    public LoanStatus Status { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];
}