using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Persistence.Ef.installments;
using LoanManagement.Services.installments.Contracts.DTOs;
using LoanManagement.Services.installments.Contracts.Interfaces;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.installments;
using LoanManagement.TestTools.Loans;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.installments;

public class InstallmentQueryTests : BusinessIntegrationTest
{
    private InstallmentQuery _sut;

    public InstallmentQueryTests()
    {
        _sut = new EFInstallmentQuery(ReadContext);
    }

    [Fact]
    public void GetById_get_a_installment_properly()
    {
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(true)
            .Build();
        Save(customer);
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        var installment = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,01,01))
            .Build();
        Save(installment);
        
        var actual = _sut.GetById(installment.Id);
        
        actual?.Status.Should().Be(installment.Status);
        actual?.DueTime.Should().Be(installment.DueTime);
        actual?.LoanId.Should().Be(installment.LoanId);
    }

    [Fact]
    public void GetAll_get_all_installments_properly()
    {
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(true)
            .Build();
        Save(customer);
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        var installment = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,01,01))
            .Build();
        Save(installment);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,02,01))
            .Build();
        Save(installment2);
        
        var actual = _sut.GetAll();
        
        actual.Should().ContainEquivalentOf(new GetAllInstallmentsDto
        {
            Id = installment.Id,
            DueTime = installment.DueTime,
            PaymentTime = installment.PaymentTime,
            Status = installment.Status,
            LoanId = installment.LoanId,
        });
        actual.Should().ContainEquivalentOf(new GetAllInstallmentsDto
        {
            Id = installment2.Id,
            DueTime = installment2.DueTime,
            PaymentTime = installment2.PaymentTime,
            Status = installment2.Status,
            LoanId = installment2.LoanId,
        });
    }

    [Fact]
    public void GetAllInstallmentsOfLoan_get_all_installments_of_loan_properly()
    {
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(true)
            .Build();
        Save(customer);
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        var installment = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,01,01))
            .Build();
        Save(installment);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,02,01))
            .Build();
        Save(installment2);
        
        var actual = _sut.GetAllInstallmentsOfLoan(loan.Id);
        
        actual.Should().ContainEquivalentOf(new GetAllInstallmentsOfLoanDto
        {
            Id = installment.Id,
            Status = installment.Status,
            DueTime = installment.DueTime,
            PaymentTime = installment.PaymentTime,
        });
        actual.Should().ContainEquivalentOf(new GetAllInstallmentsOfLoanDto
        {
            Id = installment2.Id,
            Status = installment2.Status,
            DueTime = installment2.DueTime,
            PaymentTime = installment2.PaymentTime,
        });
    }
}