using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Persistence.Ef.installments;
using LoanManagement.Persistence.Ef.Loans;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.installments;
using LoanManagement.Services.installments.Contracts.DTOs;
using LoanManagement.Services.installments.Contracts.Interfaces;
using LoanManagement.Services.installments.Exceptions;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.installments;
using LoanManagement.TestTools.Loans;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.installments;

public class InstallmentServiceTests : BusinessIntegrationTest
{
    private readonly InstallmentService _sut;

    public InstallmentServiceTests()
    {
        var loanRepository = new EFLoanRepository(SetupContext);
        var installmentRepository = new EFInstallmentRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new InstallmentAppService(loanRepository,installmentRepository,unitOfWork);
    }

    [Fact]
    public void PayInstallment_pay_installment_properly()
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
        var dto = new PayInstallmentDto()
        {
            PaymentTime = new DateOnly(2020,01,01)
        };
        
        _sut.PayInstallment(installment.Id, dto);
        
        var actual = ReadContext.Set<Installment>().Single(c => c.Id == installment.Id);
        actual.PaymentTime.Should().Be(dto.PaymentTime);
        actual.Status.Should().NotBe(InstallmentStatus.Pending);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void PayInstallment_throw_exception_when_installment_not_found(int invalidInstallmentId)
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
        
        
        var dto = new PayInstallmentDto()
        {
            PaymentTime = new DateOnly(2020,01,01)
        };
        
        var actual =()=> _sut.PayInstallment(invalidInstallmentId, dto);
        actual.Should().Throw<InstallmentNotFoundException>();
        ReadContext.Set<Installment>().Should().BeEmpty();
    }

    [Fact]
    public void PayInstallment_throw_exception_when_installment_already_paid()
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
            .WithStatus(InstallmentStatus.PaidOnTime)
            .WithDueTime(new DateOnly(2020,01,01))
            .WithPaymentTime(new DateOnly(2020,01,01))
            .Build();
        Save(installment);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.PaidOnTime)
            .WithDueTime(new DateOnly(2020,02,01))
            .WithPaymentTime(new DateOnly(2020,02,01))
            .Build();
        Save(installment2);
        var dto = new PayInstallmentDto()
        {
            PaymentTime = new DateOnly(2020,02,02)
        };
        
        var actual =()=> _sut.PayInstallment(installment.Id, dto);

        actual.Should().ThrowExactly<InstallmentAlreadyPaidException>();
        var result = ReadContext.Set<Installment>().Single(c => c.Id == installment.Id);
        result.PaymentTime.Should().Be(installment.PaymentTime);
        result.Status.Should().Be(InstallmentStatus.PaidOnTime);
    }
}