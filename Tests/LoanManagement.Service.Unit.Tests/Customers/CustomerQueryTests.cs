using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Persistence.Ef.Customers;
using LoanManagement.Services.Customers.Contracts.DTOs;
using LoanManagement.Services.Customers.Contracts.Interfaces;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using Xunit;

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
            JobType = customer.JobType,
            MonthlyIncome = customer.MonthlyIncome,
            Email = customer.Email,
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
            JobType = customer2.JobType,
            MonthlyIncome = customer2.MonthlyIncome,
            Email = customer2.Email,
            Assets = customer2.Assets,
            IsVerified = customer2.IsVerified,
        });
    }
}