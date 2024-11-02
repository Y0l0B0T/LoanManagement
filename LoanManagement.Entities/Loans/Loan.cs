using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.LoansDefinition;

namespace LoanManagement.Entities.Loans;

public class Loan
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int LoanDefinitionId { get; set; }
    public DateOnly CreationDate { get; set; }
    public int ValidationScore { get; set; }
    public LoanStatus Status { get; set; }
    public string LoanType { get; set; }
    public HashSet<Installment> Installments { get; set; } = [];

    public Customer Customer { get; set; }
    public LoanDefinition LoanDefinition { get; set; }
}
public enum LoanStatus
{
    UnderReview = 1,
    Approved,
    Rejected,
    Paying,
    DelayInPaying,
    Closed
}
