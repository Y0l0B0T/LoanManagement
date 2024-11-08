using System.ComponentModel.DataAnnotations;
using LoanManagement.Entities.Loans;

namespace LoanManagement.Services.Loans.Contracts.DTOs;

public class AddLoanDto
{
    [Range(1, Int32.MaxValue)] public int CustomerId { get; set; }
    [Range(1, Int32.MaxValue)] public int LoanDefinitionId { get; set; }
}