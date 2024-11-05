using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.Loans;
using LoanManagement.Persistence.Ef.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.Loans;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.Loans;

public class LoanQueryTests: BusinessIntegrationTest
{
    private readonly LoanQuery _sut;

    public LoanQueryTests()
    {
        _sut = new EFLoanQuery(ReadContext);
    }

    [Fact]
    public void GetById_get_a_loan_properly()
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
            .WithStatus(LoanStatus.UnderReview)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual = _sut.GetById(loan.Id);
        
        actual?.CustomerId.Should().Be(customer.Id);
        actual?.LoanDefinitionId.Should().Be(loanDefinition.Id);
        actual?.ValidationScore.Should().Be(65);
    }

    [Fact]
    public void GetAll_get_all_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.UnderReview)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.UnderReview)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAll();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = loan.Status,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = loan2.Status,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }

    [Fact]
    public void GetAllInReview_get_all_under_review_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.UnderReview)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.UnderReview)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAllInReview();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.UnderReview,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.UnderReview,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }

    [Fact]
    public void GetAllInApproved_get_all_approved_loans_properly()
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

        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Approved)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);

        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.Approved)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);

        var actual = _sut.GetAllInApproved();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.Approved,
            CreationDate = new DateOnly(2020, 01, 01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.Approved,
            CreationDate = new DateOnly(2020, 01, 01)
        });
    }
    [Fact]
    public void GetAllInRejected_get_all_rejected_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Rejected)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.Rejected)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAllInRejected();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.Rejected,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.Rejected,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }
    [Fact]
    public void GetAllInPaying_get_all_in_paying_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAllInPaying();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.Paying,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.Paying,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }
    [Fact]
    public void GetAllActiveLoans_get_all_active_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.DelayInPaying)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAllActiveLoans();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.DelayInPaying,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.Paying,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }
    [Fact]
    public void GetAllInClosed_get_all_closed_loans_properly()
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
        
        var loanDefinition2 = new LoanDefinitionBuilder()
            .WithInstallmentsCount(16)
            .WithLoanAmount(20000000).Build();
        Save(loanDefinition2);

        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .WithValidationScore(70)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition2.Id)
            .WithStatus(LoanStatus.Closed)
            .WithValidationScore(80)
            .WithLoanType("LongTerm")
            .Build();
        Save(loan2);
        
        var actual = _sut.GetAllInClosed();

        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan.Id,
            ValidationScore = loan.ValidationScore,
            LoanDefinitionId = loan.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan.Installments,
            LoanType = loan.LoanType,
            Status = LoanStatus.Closed,
            CreationDate = new DateOnly(2020, 01,01)
        });
        actual.Should().ContainEquivalentOf(new GetAllLoansDto
        {
            Id = loan2.Id,
            ValidationScore = loan2.ValidationScore,
            LoanDefinitionId = loan2.LoanDefinitionId,
            CustomerId = customer.Id,
            Installments = loan2.Installments,
            LoanType = loan2.LoanType,
            Status = LoanStatus.Closed,
            CreationDate = new DateOnly(2020, 01,01)
        });
    }
}