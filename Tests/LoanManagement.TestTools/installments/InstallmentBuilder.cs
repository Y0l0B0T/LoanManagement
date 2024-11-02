using LoanManagement.Entities.installments;

namespace LoanManagement.TestTools.installments;

public class InstallmentBuilder
{
    private Installment _installment;

    public InstallmentBuilder()
    {
        _installment = new Installment()
        {
            LoanId = 1,
            DueTime = new DateOnly(2022,01,01),
        };
    }
    public InstallmentBuilder WithLoanId(int loanId)
    {
        _installment.LoanId = loanId;
        return this;
    }

    public InstallmentBuilder WithDueTime(DateOnly dueTime)
    {
        _installment.DueTime = dueTime;
        return this;
    }

    public InstallmentBuilder WithPaymentTime(DateOnly paymentTime)
    {
        _installment.PaymentTime = paymentTime;
        return this;
    }
    
    public Installment Build()
    {
        return _installment;
    }
}