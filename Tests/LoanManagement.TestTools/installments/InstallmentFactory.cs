using LoanManagement.Entities.installments;

namespace LoanManagement.TestTools.installments;

public class InstallmentFactory
{
    public static Installment Create(int loanId, DateOnly dueTime, InstallmentStatus status,
        DateOnly? paymentTime = null)
    {
        return new Installment()
        {
            DueTime = dueTime,
            Status = status,
            LoanId = loanId,
            PaymentTime = paymentTime,
        };
    }
}