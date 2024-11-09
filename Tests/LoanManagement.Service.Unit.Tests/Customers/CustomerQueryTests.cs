using LoanManagement.Persistence.Ef.Customers;

namespace LoanManagement.Service.Unit.Tests.Customers;

public class CustomerQueryTests : BusinessIntegrationTest
{
    private readonly CustomerQuery _sut;

    public CustomerQueryTests()
    {
        _sut = new EFCustomerQuery(ReadContext);
    }

    [Fact]
    public void GetById_get_a_customer_properly()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var customer2 = new CustomerBuilder()
            .WithPhoneNumber("0987654321")
            .WithNationalCode("987654322")
            .Build();
        Save(customer2);
        
        var actual = _sut.GetById(customer.Id);
        
        actual?.Id.Should().Be(customer.Id);
        actual?.FirstName.Should().Be(customer.FirstName);
        actual?.LastName.Should().Be(customer.LastName);
        actual?.PhoneNumber.Should().Be(customer.PhoneNumber);
        actual?.NationalCode.Should().Be(customer.NationalCode);
        actual?.Email.Should().Be(customer.Email);
        actual?.Documents.Should().Be(customer.Documents);
    }

    [Fact]
    public void GetAll_get_all_customers_properly()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var customer2 = new CustomerBuilder()
            .WithPhoneNumber("0987654321")
            .WithNationalCode("987654322")
            .Build();
        Save(customer2);

        var actual = _sut.GetAll();

        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllCustomersDto()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            NationalCode = customer.NationalCode,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Documents = customer.Documents,
            JobType = customer.JobType,
            MonthlyIncome = customer.MonthlyIncome,
            Assets = customer.Assets,
            IsVerified = customer.IsVerified,
        });
        actual.Should().ContainEquivalentOf(new GetAllCustomersDto()
        {
            Id = customer2.Id,
            FirstName = customer2.FirstName,
            LastName = customer2.LastName,
            NationalCode = customer2.NationalCode,
            PhoneNumber = customer2.PhoneNumber,
            Email = customer2.Email,
            Documents = customer2.Documents,
            JobType = customer2.JobType,
            MonthlyIncome = customer2.MonthlyIncome,
            Assets = customer2.Assets,
            IsVerified = customer2.IsVerified,
        });
    }

    [Fact]
    public void GetAllCustomersPendingForConfirmation_get_all_customers_pending_for_confirm_properly()
    {
        var customer = new CustomerBuilder()
            .WithDocuments("madrak")
            .IsVerified(false)
            .Build();
        Save(customer);
        var customer2 = new CustomerBuilder()
            .WithPhoneNumber("0987654321")
            .WithNationalCode("987654322")
            .WithDocuments("madrak")
            .IsVerified(false)
            .Build();
        Save(customer2);

        var actual = _sut.GetAllCustomersPendingForConfirmation();
        
        actual.Should().HaveCount(2);
        actual.Should().ContainEquivalentOf(new GetAllCustomersPendingForConfirmation()
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            NationalCode = customer.NationalCode,
            PhoneNumber = customer.PhoneNumber,
            Email = customer.Email,
            Documents = customer.Documents,
        });
        actual.Should().ContainEquivalentOf(new GetAllCustomersPendingForConfirmation()
        {
            Id = customer2.Id,
            FirstName = customer2.FirstName,
            LastName = customer2.LastName,
            NationalCode = customer2.NationalCode,
            PhoneNumber = customer2.PhoneNumber,
            Email = customer2.Email,
            Documents = customer2.Documents,
        });
    }
    [Theory]
    [InlineData("2280472158")]
    [InlineData("1111111111")]
    public void GetByNationalCode_get_a_customer_with_national_code_properly(string nationalCode)
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);
        var customer2 = new CustomerBuilder()
            .WithPhoneNumber("0987654321")
            .WithNationalCode(nationalCode)
            .Build();
        Save(customer2);
        
        var actual = _sut.GetByNationalCode(nationalCode);
        
        actual?.Id.Should().Be(customer2.Id);
        actual?.FirstName.Should().Be(customer2.FirstName);
        actual?.LastName.Should().Be(customer2.LastName);
        actual?.PhoneNumber.Should().Be(customer2.PhoneNumber);
        actual?.NationalCode.Should().Be(customer2.NationalCode);
        actual?.Email.Should().Be(customer2.Email);
        actual?.Documents.Should().Be(customer2.Documents);
    }

    [Fact]
    public void GetLoansByNationalCode_get_a_customer_all_loans_with_national_code_properly()
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
        
        var actual = _sut.GetLoansByNationalCode(customer.NationalCode);
        
        actual.Should().HaveCount(1);
        actual.Should().ContainEquivalentOf(new GetLoansByNationalCodeDto()
        {
            Status = loan.Status,
            LoanType = loan.LoanType,
            InstallmentsCount = loan.Installments.Count
        });

    }
    
}