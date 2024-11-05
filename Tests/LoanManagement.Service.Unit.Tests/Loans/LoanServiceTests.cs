using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.Loans;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.Customers.Exceptions;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using LoanManagement.Services.LoansDefinition.Exceptions;
using LoanManagement.TestTools.Admins;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.Loans;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.Loans;

public class LoanServiceTests : BusinessIntegrationTest
{
    private readonly LoanService _sut;

    public LoanServiceTests()
    {
        _sut = LoanServiceFactory.Generate(SetupContext);
    }

    [Fact]
    public void Add_add_a_loan_properly()
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
        
        var dto = new AddLoanDto()
        {
            CustomerId = customer.Id,
            LoanDefinitionId = loanDefinition.Id,
        };

        _sut.Add(dto);

        var actual = ReadContext.Set<Loan>().Single();
        actual.Should().NotBeNull();
        actual.CustomerId.Should().Be(dto.CustomerId);
        actual.LoanDefinitionId.Should().Be(dto.LoanDefinitionId);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Add_throw_exception_when_customer_not_found(int invalidCustomerId)
    {
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        
        var dto = new AddLoanDto()
        {
            CustomerId = invalidCustomerId,
            LoanDefinitionId = loanDefinition.Id,
        };

        var actual =()=> _sut.Add(dto);

        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void Add_throw_exception_when_customer_is_not_verified()
    {
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(false)
            .Build();
        Save(customer);
        
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        
        var dto = new AddLoanDto()
        {
            CustomerId = customer.Id,
            LoanDefinitionId = loanDefinition.Id,
        };

        var actual =()=> _sut.Add(dto);
        actual.Should().ThrowExactly<CustomerIsNotVerifiedException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Add_throw_when_loan_definition_is_not_found(int invalidLoanDefinitionId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(true)
            .Build();
        Save(customer);
        
        
        var dto = new AddLoanDto()
        {
            CustomerId = customer.Id,
            LoanDefinitionId = invalidLoanDefinitionId,
        };

        var actual =()=> _sut.Add(dto);
        actual.Should().ThrowExactly<LoanDefinitionNotFoundException>();
    }

    [Fact]
    public void Add_throw_exception_when_loan_is_duplicate()
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var dto = new AddLoanDto()
        {
            CustomerId = customer.Id,
            LoanDefinitionId = loanDefinition.Id,
        };

        var actual =()=> _sut.Add(dto);

        actual.Should().ThrowExactly<LoanDuplicatedException>();
        ReadContext.Set<Loan>().Should().HaveCount(1)
            .And.ContainSingle(l=>l.CustomerId==customer.Id);
    }

    [Fact]
    public void ConfirmLoan_confirm_a_loan_properly()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
        
        _sut.ConfirmLoan(admin.Id,loan.Id);
        

        var actual = ReadContext.Set<Loan>().Single();
        actual.Status.Should().Be(LoanStatus.Approved);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void ConfirmLoan_throw_exception_when_admin_not_found(int invalidAdminId)
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
        
        var actual =()=> _sut.ConfirmLoan(invalidAdminId,loan.Id);
        
        actual.Should().ThrowExactly<AdminNotFoundException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.UnderReview);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void ConfirmLoan_throw_exception_when_loan_not_found(int invalidLoanId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var actual =()=> _sut.ConfirmLoan(admin.Id,invalidLoanId);
        
        actual.Should().ThrowExactly<LoanNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void ConfirmLoan_throw_exception_when_loan_status_invalid_for_confirm()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
        
        var actual =()=> _sut.ConfirmLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<InvalidLoanStatusForConfirmationException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Paying);
    }
    [Fact]
    public void RejectLoan_reject_a_loan_properly()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
        
        _sut.RejectLoan(admin.Id,loan.Id);
        

        var actual = ReadContext.Set<Loan>().Single();
        actual.Status.Should().Be(LoanStatus.Rejected);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void RejectLoan_throw_exception_when_admin_not_found(int invalidAdminId)
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
        
        var actual =()=> _sut.RejectLoan(invalidAdminId,loan.Id);
        
        actual.Should().ThrowExactly<AdminNotFoundException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.UnderReview);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void RejectLoan_throw_exception_when_loan_not_found(int invalidLoanId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var actual =()=> _sut.RejectLoan(admin.Id,invalidLoanId);
        
        actual.Should().ThrowExactly<LoanNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void RejectLoan_throw_exception_when_loan_status_invalid_for_reject()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithStatus(LoanStatus.Closed)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.RejectLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<InvalidLoanStatusForRejectionException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Closed);
    }
    [Fact]
    public void PayLoan_pay_a_loan_properly()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithStatus(LoanStatus.Approved)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        _sut.PayLoan(admin.Id,loan.Id);
        

        var actual = ReadContext.Set<Loan>().Single();
        actual.Status.Should().Be(LoanStatus.Paying);
        actual.Installments.Should().HaveCount(6);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void PayLoan_throw_exception_when_admin_not_found(int invalidAdminId)
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
            .WithStatus(LoanStatus.Approved)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.PayLoan(invalidAdminId,loan.Id);
        
        actual.Should().ThrowExactly<AdminNotFoundException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Approved);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void PayLoan_throw_exception_when_loan_not_found(int invalidLoanId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var actual =()=> _sut.PayLoan(admin.Id,invalidLoanId);
        
        actual.Should().ThrowExactly<LoanNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void PayLoan_throw_exception_when_loan_status_invalid_for_pay()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithStatus(LoanStatus.Rejected)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.PayLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<InvalidLoanStatusForPayingException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Rejected);
    }
    [Fact]
    public void DelayInPayLoan_Delay_in_pay_a_loan_properly()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        _sut.DelayInPayLoan(admin.Id,loan.Id);
        

        var actual = ReadContext.Set<Loan>().Single();
        actual.Status.Should().Be(LoanStatus.DelayInPaying);
        actual.Installments.Should().NotBeEmpty();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void DelayInPayLoan_throw_exception_when_admin_not_found(int invalidAdminId)
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
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.DelayInPayLoan(invalidAdminId,loan.Id);
        
        actual.Should().ThrowExactly<AdminNotFoundException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Paying);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void DelayInPayLoan_throw_exception_when_loan_not_found(int invalidLoanId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var actual =()=> _sut.DelayInPayLoan(admin.Id,invalidLoanId);
        
        actual.Should().ThrowExactly<LoanNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void DelayInPayLoan_throw_exception_when_loan_status_invalid_for_delay_in_pay()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithStatus(LoanStatus.Rejected)
            .WithValidationScore(65)
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.DelayInPayLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<InvalidLoanStatusForDelayInPayException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Rejected);
    }
    [Fact]
    public void ClosedLoan_close_a_loan_properly()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        _sut.ClosedLoan(admin.Id,loan.Id);
        

        var actual = ReadContext.Set<Loan>().Single();
        actual.Status.Should().Be(LoanStatus.Closed);
        actual.Installments.Should().BeEmpty();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void ClosedLoan_throw_exception_when_admin_not_found(int invalidAdminId)
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
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.ClosedLoan(invalidAdminId,loan.Id);
        
        actual.Should().ThrowExactly<AdminNotFoundException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Paying);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void ClosedLoan_throw_exception_when_loan_not_found(int invalidLoanId)
    {
        var admin = AdminFactory.Create();
        Save(admin);
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
        
        var actual =()=> _sut.ClosedLoan(admin.Id,invalidLoanId);
        
        actual.Should().ThrowExactly<LoanNotFoundException>();
        ReadContext.Set<Loan>().Should().BeEmpty();
    }

    [Fact]
    public void ClosedLoan_throw_exception_when_loan_status_invalid_for_closed()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithStatus(LoanStatus.Rejected)
            .WithValidationScore(65)
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        
        var actual =()=> _sut.DelayInPayLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<InvalidLoanStatusForDelayInPayException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Rejected);
    }
    [Fact]
    public void ClosedLoan_throw_exception_when_installment_list_is_not_empty()
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
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
            .WithInstallments()
            .WithLoanType("ShortTerm")
            .Build();
        
        Save(loan);
        
        var actual =()=> _sut.DelayInPayLoan(admin.Id,loan.Id);
        actual.Should().ThrowExactly<LoanHasInstallmentsException>();
        var result = ReadContext.Set<Loan>().Single();
        result.Status.Should().Be(LoanStatus.Paying);
        result.Installments.Should().NotBeEmpty();
    }
    
    
}