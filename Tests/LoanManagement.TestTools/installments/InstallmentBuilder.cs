namespace LoanManagement.TestTools.installments;

public class InstallmentBuilder
{
    private readonly Installment _installment;

    public InstallmentBuilder()
    {
        _installment = new Installment()
        {
            DueTime = new DateOnly(2020, 01, 01),
            Status = InstallmentStatus.Pending,
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

    public InstallmentBuilder WithStatus(InstallmentStatus status)
    {
        _installment.Status = status;
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