using LoanManagement.Entities.Loans;

namespace LoanManagement.Entities.installments;

public class Installment
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public DateOnly DueTime { get; set; }
    public DateOnly? PaymentTime { get; set; }
        
    public Loan Loan { get; set; }
}