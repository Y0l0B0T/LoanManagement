using FluentAssertions;
using LoanManagement.Persistence.Ef.LoansDefinition;
using LoanManagement.Services.LoansDefinition.Contracts.DTOs;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.LoansDefinition;

public class LoanDefinitionQueryTests : BusinessIntegrationTest
{
    private readonly LoanDefinitionQuery _sut;

    public LoanDefinitionQueryTests()
    {
        _sut = new EFLoanDefinitionQuery(ReadContext);
    }

    [Fact]
    public void GetById_get_a_loan_definition_properly()
    {
        var loanDefinition = new LoanDefinitionBuilder().Build();
        Save(loanDefinition);
        
        var loanDefinition2 = new LoanDefinitionBuilder().
            WithLoanAmount(50000000)
            .WithInstallmentsCount(18).Build();
        Save(loanDefinition2);
        
        var actual = _sut.GetById(loanDefinition.Id);
        
        actual?.Id.Should().Be(loanDefinition.Id);
        actual?.Name.Should().Be(loanDefinition.Name);
        actual?.LoanAmount.Should().Be(loanDefinition.LoanAmount);
        actual?.InstallmentsCount.Should().Be(loanDefinition.InstallmentsCount);
        actual?.InterestRate.Should().Be(loanDefinition.InterestRate);
        actual?.InstallmentAmount.Should().Be(loanDefinition.InstallmentAmount);
        actual?.BasePaymentAmount.Should().Be(loanDefinition.BasePaymentAmount);
        actual?.MonthlyInterestAmount.Should().Be(loanDefinition.MonthlyInterestAmount);
        actual?.MonthlyPenaltyAmount.Should().Be(loanDefinition.MonthlyPenaltyAmount);
    }

    [Fact]
    public void GetAll_get_all_loans_definition_properly()
    {
        var loanDefinition = new LoanDefinitionBuilder().Build();
        Save(loanDefinition);
        
        var loanDefinition2 = new LoanDefinitionBuilder().
            WithLoanAmount(50000000)
            .WithInstallmentsCount(18).Build();
        Save(loanDefinition2);

        var actual = _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllLoanDefinitionDto()
        {
            Id = loanDefinition.Id,
            Name = loanDefinition.Name,
            LoanAmount = loanDefinition.LoanAmount,
            InterestRate = loanDefinition.InterestRate,
            InstallmentsCount = loanDefinition.InstallmentsCount,
            InstallmentAmount = loanDefinition.InstallmentAmount,
            BasePaymentAmount = loanDefinition.BasePaymentAmount,
            MonthlyInterestAmount = loanDefinition.MonthlyInterestAmount,
            MonthlyPenaltyAmount = loanDefinition.MonthlyPenaltyAmount,
        });
        actual.Should().ContainEquivalentOf(new GetAllLoanDefinitionDto()
        {
            Id = loanDefinition2.Id,
            Name = loanDefinition2.Name,
            LoanAmount = loanDefinition2.LoanAmount,
            InterestRate = loanDefinition2.InterestRate,
            InstallmentsCount = loanDefinition2.InstallmentsCount,
            InstallmentAmount = loanDefinition2.InstallmentAmount,
            BasePaymentAmount = loanDefinition2.BasePaymentAmount,
            MonthlyInterestAmount = loanDefinition2.MonthlyInterestAmount,
            MonthlyPenaltyAmount = loanDefinition2.MonthlyPenaltyAmount,
        });
    }
}