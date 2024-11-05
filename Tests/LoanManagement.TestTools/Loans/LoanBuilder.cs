using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;

namespace LoanManagement.TestTools.Loans;

public class LoanBuilder
{
    private readonly Loan _loan;

    public LoanBuilder()
    {
        _loan = new Loan
        {
            CreationDate = new DateOnly(2020, 01,01),
            Status = LoanStatus.UnderReview,
        };
    }

    public LoanBuilder WithCustomerId(int customerId)
    {
        _loan.CustomerId = customerId;
        return this;
    }

    public LoanBuilder WithLoanDefinitionId(int loanDefinitionId)
    {
        _loan.LoanDefinitionId = loanDefinitionId;
        return this;
    }

    public LoanBuilder WithLoanType(string loanType)
    {
        _loan.LoanType = loanType;
        return this;
    }

    public LoanBuilder WithInstallments()
    {
        _loan.Installments = new HashSet<Installment>();
        return this;
    }

    public LoanBuilder WithValidationScore(int score)
    {
        _loan.ValidationScore = score;
        return this;
    }

    public LoanBuilder WithStatus(LoanStatus status)
    {
        _loan.Status = status;
        return this;
    }

    public Loan Build()
    {
        return _loan;
    }
}