using LoanManagement.Entities.Loans;

namespace LoanManagement.Entities.LoansDefinition;

public class LoanDefinition
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal LoanAmount { get; set; }
    public int InstallmentsCount { get; set; }
    public decimal InterestRate { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal BasePaymentAmount { get; set; }
    public decimal MonthlyInterestAmount { get; set; }
    public decimal MonthlyPenaltyAmount { get; set; }

    public HashSet<Loan> AllLoans { get; set; } = [];
}