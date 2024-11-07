using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.LoansDefinition;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

namespace LoanManagement.TestTools.LoansDefinition;

public class LoanDefinitionBuilder
{
    private LoanDefinition _loanDefinition;

    public LoanDefinitionBuilder()
    {
        _loanDefinition = new LoanDefinition
        {
            LoanAmount = 100.00m,
            InstallmentsCount = 4,
            Name = "ShortTerm"
        };
    }

    public LoanDefinitionBuilder WithLoanAmount(decimal amount)
    {
        _loanDefinition.LoanAmount = Math.Round(amount, 2);
        _loanDefinition.MonthlyInterestAmount = CalculateMonthlyInterestAmount(amount, _loanDefinition.InterestRate);
        _loanDefinition.BasePaymentAmount = CalculateBasePaymentAmount(amount, _loanDefinition.InstallmentsCount);
        _loanDefinition.InstallmentAmount = CalculateInstallmentAmount(_loanDefinition.BasePaymentAmount,
            _loanDefinition.MonthlyInterestAmount);
        _loanDefinition.MonthlyPenaltyAmount = CalculateMonthlyPenaltyAmount(_loanDefinition.InstallmentAmount);
        return this;
    }

    public LoanDefinitionBuilder WithInstallmentsCount(int count)
    {
        _loanDefinition.InstallmentsCount = count;
        _loanDefinition.InterestRate = count <= 12 ? 0.15m : 0.20m;
        _loanDefinition.Name = count <= 12 ? "ShortTerm" : "LongTerm";
        _loanDefinition.MonthlyInterestAmount =
            CalculateMonthlyInterestAmount(_loanDefinition.LoanAmount, _loanDefinition.InterestRate);
        _loanDefinition.BasePaymentAmount = CalculateBasePaymentAmount(_loanDefinition.LoanAmount, count);
        _loanDefinition.InstallmentAmount = CalculateInstallmentAmount(_loanDefinition.BasePaymentAmount,
            _loanDefinition.MonthlyInterestAmount);
        _loanDefinition.MonthlyPenaltyAmount = CalculateMonthlyPenaltyAmount(_loanDefinition.InstallmentAmount);
        return this;
    }

    public LoanDefinition Build()
    {
        return _loanDefinition;
    }

    //-----------------------------------------Private Method For Calculating Properties :)
    private decimal CalculateMonthlyInterestAmount(decimal loanAmount, decimal interestRate)
    {
        return Math.Round((interestRate / 12) * loanAmount, 2);
    }

    private decimal CalculateBasePaymentAmount(decimal loanAmount, int installmentsCount)
    {
        return Math.Round(loanAmount / installmentsCount, 2);
    }

    private decimal CalculateInstallmentAmount(decimal basePaymentAmount, decimal monthlyInterestAmount)
    {
        return Math.Round(basePaymentAmount + monthlyInterestAmount, 2);
    }

    private decimal CalculateMonthlyPenaltyAmount(decimal installmentAmount)
    {
        return Math.Round(0.02m * installmentAmount, 2);
    }
}