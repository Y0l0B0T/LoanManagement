using LoanManagement.Entities.Loans;

namespace LoanManagement.TestTools.Loans;

public class LoanBuilder
{
    private readonly Loan _loan;

    public LoanBuilder()
    {
        _loan = new Loan()
        {
            CustomerId = 1,
            LoanDefinitionId = 1,
            CreationDate = new DateOnly(2022,01,01),
            ValidationScore = 0,
            Status = LoanStatus.UnderReview,
            LoanType = "Short-Term"
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

    public LoanBuilder WithValidationScore(int validationScore)
    {
        _loan.ValidationScore = validationScore;
        return this;
    }

    public LoanBuilder WithStatus(LoanStatus status)
    {
        _loan.Status = status;
        return this;
    }

    public LoanBuilder WithLoanType(string loanType)
    {
        _loan.LoanType = loanType;
        return this;
    }

    public Loan Build()
    {
        return _loan;
    }
}