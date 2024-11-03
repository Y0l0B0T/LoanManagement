using LoanManagement.Entities.LoansDefinition;

namespace LoanManagement.TestTools.LoansDefinition;

public class LoanDefinitionBuilder
{
    private readonly LoanDefinition _loanDefinition;

    public LoanDefinitionBuilder()
    {
        _loanDefinition = new LoanDefinition()
        {
            LoanAmount = 10000000,
            InterestRate = 15M,
            InstallmentsCount = 12,
            InstallmentAmount = 958300,
            MonthlyInterestAmount = 125000,
            MonthlyPenaltyAmount = 0,
        };
    }

    public LoanDefinitionBuilder WithLoanAmount(decimal loanAmount)
    {
        _loanDefinition.LoanAmount = loanAmount;
        return this;
    }

    public LoanDefinitionBuilder WithInterestRate(decimal interestRate)
    {
        _loanDefinition.InterestRate = interestRate;
        return this;
    }

    public LoanDefinitionBuilder WithInstallmentsCount(int installmentsCount)
    {
        _loanDefinition.InstallmentsCount = installmentsCount;
        return this;
    }

    public LoanDefinitionBuilder WithInstallmentAmount(decimal installmentAmount)
    {
        _loanDefinition.InstallmentAmount = installmentAmount;
        return this;
    }

    public LoanDefinitionBuilder WithMonthlyInterestAmount(decimal monthlyInterestAmount)
    {
        _loanDefinition.MonthlyInterestAmount = monthlyInterestAmount;
        return this;
    }

    public LoanDefinitionBuilder WithMonthlyPenaltyAmount(decimal monthlyPenaltyAmount)
    {
        _loanDefinition.MonthlyPenaltyAmount = monthlyPenaltyAmount;
        return this;
    }

    public LoanDefinition Build()
    {
        return _loanDefinition;
    }
}