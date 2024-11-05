using LoanManagement.Entities.Customers;
using LoanManagement.Entities.Loans;

namespace LoanManagement.TestTools.Customers;

public class CustomerBuilder
{
    private readonly Customer _customer;

    public CustomerBuilder()
    {
        _customer = new Customer
        {
            FirstName = "Esmaeel",
            LastName = "Kermani",
            PhoneNumber = "09176711365",
            NationalCode = "2480472159"
        };
    }
    
    public CustomerBuilder WithFirstName(string firstName)
    {
        _customer.FirstName = firstName;
        return this;
    }

    public CustomerBuilder WithLastName(string lastName)
    {
        _customer.LastName = lastName;
        return this;
    }

    public CustomerBuilder WithPhoneNumber(string phoneNumber)
    {
        _customer.PhoneNumber = phoneNumber;
        return this;
    }

    public CustomerBuilder WithNationalCode(string nationalCode)
    {
        _customer.NationalCode = nationalCode;
        return this;
    }

    public CustomerBuilder WithEmail(string email)
    {
        _customer.Email = email;
        return this;
    }

    public CustomerBuilder WithDocuments(string documents)
    {
        _customer.Documents = documents;
        return this;
    }
    public CustomerBuilder IsVerified(bool isVerified)
    {
        _customer.IsVerified = isVerified;
        return this;
    }

    public CustomerBuilder WithMonthlyIncome(decimal monthlyIncome)
    {
        _customer.MonthlyIncome = monthlyIncome;
        return this;
    }

    public CustomerBuilder WithJobType(JobType jobType)
    {
        _customer.JobType = jobType;
        return this;
    }

    public CustomerBuilder WithAssets(decimal assets)
    {
        _customer.Assets = assets;
        return this;
    }

    public Customer Build()
    {
        return _customer;
    }
}