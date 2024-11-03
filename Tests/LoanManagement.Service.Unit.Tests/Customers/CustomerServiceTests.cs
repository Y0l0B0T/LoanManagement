﻿using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Persistence.Ef.Customers;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.Customers;
using LoanManagement.Services.Customers.Contracts.DTOs;
using LoanManagement.Services.Customers.Contracts.Interfaces;
using LoanManagement.Services.Customers.Exceptions;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.Customers;

public class CustomerServiceTests : BusinessIntegrationTest
{
    private readonly CustomerService _sut;

    public CustomerServiceTests()
    {
        var customerRepository = new EFCustomerRepository(SetupContext);
        var unitOfWork = new EfUnitOfWork(SetupContext);
        _sut = new CustomerAppService(customerRepository, unitOfWork);
    }

    [Fact]
    public void Add_add_a_customer_properly()
    {
        var dto = new AddCustomerDto
        {
            FirstName = "Esmaeel",
            LastName = "Kermani",
            NationalCode = "2480472159",
            PhoneNumber = "09176711365",
            Email = "Kermani.esmaeel@gmail.com"
        };

        _sut.Add(dto);

        var actual = ReadContext.Set<Customer>().Single();
        actual.Should().BeEquivalentTo(
            new CustomerBuilder()
                .WithFirstName(dto.FirstName)
                .WithLastName(dto.LastName)
                .WithNationalCode(dto.NationalCode)
                .WithPhoneNumber(dto.PhoneNumber)
                .WithEmail(dto.Email).Build(),c=>c.Excluding(x=>x.Id));
    }
    [Theory]
    [InlineData("2480472159")]
    [InlineData("1111111111")]
    public void Add_throw_exception_when_customer_national_code_is_duplicate(string duplicateNationalCode)
    {
        var customer = new CustomerBuilder().WithNationalCode(duplicateNationalCode).Build();
        Save(customer);
        
        var dto = new AddCustomerDto
        {
            FirstName = "Esmaeel",
            LastName = "Kermani",
            NationalCode = duplicateNationalCode,
            PhoneNumber = "12345678901",
            Email = "Kermani.esmaeel@gmail.com"
        };
        
        var actual =()=> _sut.Add(dto);

        actual.Should().ThrowExactly<NationalCodeDuplicatedException>();
        ReadContext.Set<Customer>().Should().HaveCount(1)
            .And.ContainSingle(c => c.NationalCode == duplicateNationalCode);
    }

    [Theory]
    [InlineData("09176711365")]
    [InlineData("09171111111")]
    public void Add_throw_exception_when_customer_phone_number_is_duplicate(string duplicatePhoneNumber)
    {
        var customer = new CustomerBuilder().WithPhoneNumber(duplicatePhoneNumber).Build();
        Save(customer);
        
        var dto = new AddCustomerDto
        {
            FirstName = "Esmaeel",
            LastName = "Kermani",
            NationalCode = "2480472000",
            PhoneNumber = duplicatePhoneNumber,
            Email = "Kermani.esmaeel@gmail.com"
        };
        
        var actual  = () => _sut.Add(dto);

        actual.Should().ThrowExactly<PhoneNumberDuplicatedException>();
        ReadContext.Set<Customer>().Should().HaveCount(1)
            .And.ContainSingle(c => c.PhoneNumber == duplicatePhoneNumber);
    }
    
    [Fact]
    public void AddDocuments_add_a_documents_to_customer_properly()
    {
        var customer = new CustomerBuilder().Build();
        Save(customer);

        var dto = new AddDocumentsDto()
        {
            Documents = "www.uplod.ir/madrak1.jpg",
        };
        
        _sut.AddDocuments(customer.Id, dto);
        
        var actual = ReadContext.Set<Customer>().FirstOrDefault(x => x.Id == customer.Id);
        actual?.Documents.Should().BeEquivalentTo(dto.Documents);
    }
    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void AddDocuments_throw_exception_when_customer_is_not_found(int invalidCustomerId)
    {
        var dto = new AddDocumentsDto()
        {
            Documents = "www.uplod.ir/madrak1.jpg",
        };
        
        var actual =()=> _sut.AddDocuments(invalidCustomerId,dto);
        
        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Customer>().Should().BeEmpty();
    }
    
    [Fact]
    public void AddFinancialInfo_add_a_financial_information_to_customer_properly()
    {
        var customer = new CustomerBuilder()
            .IsVerified(true).Build();
        Save(customer);

        var dto = new AddFinancialInfoDto()
        {
            MonthlyIncome = 1000m,
            JobType = JobType.SelfEmployed,
            Assets = 10000m
        };
        
        _sut.AddFinancialInfo(customer.Id,dto);
        
        var actual = ReadContext.Set<Customer>().FirstOrDefault(x => x.Id == customer.Id);
        actual?.MonthlyIncome.Should().Be(dto.MonthlyIncome);
        actual?.JobType.Should().Be(dto.JobType);
        actual?.Assets.Should().Be(dto.Assets);
    }
    [Fact]
    public void AddFinancialInfo_throw_exception_when_customer_is_not_verified()
    {
        var customer = new CustomerBuilder().IsVerified(false).Build();
        Save(customer);

        var dto = new AddFinancialInfoDto()
        {
            JobType = JobType.SelfEmployed,
            Assets = 10000m,
            MonthlyIncome = 1000m
        };
        
        var actual =()=> _sut.AddFinancialInfo(customer.Id,dto);
        
        actual.Should().ThrowExactly<CustomerIsNotVerifiedException>();
        ReadContext.Set<Customer>().Should().HaveCount(1)
            .And.Contain(c => c.Id == customer.Id &&
                              c.JobType != JobType.SelfEmployed &&
                              c.IsVerified == false &&
                              c.Assets != dto.Assets &&
                              c.MonthlyIncome != dto.MonthlyIncome);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void AddFinancialInfo_throw_exception_when_customer_is_not_found(int invalidCustomerId)
    {
        var dto = new AddFinancialInfoDto()
        {
            JobType = JobType.SelfEmployed,
            Assets = 10000m,
            MonthlyIncome = 1000m
        };
        
        var actual =()=> _sut.AddFinancialInfo(invalidCustomerId,dto);
        
        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Customer>().Should().BeEmpty();
    }
    
    
    [Fact]
    public void Confirmed_confirm_a_customer_verification_properly()
    {
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        
        _sut.Confirmed(customer1.Id);
        
        var actual = ReadContext.Set<Customer>().FirstOrDefault(x => x.Id == customer1.Id);
        actual.Should().NotBeNull();
        actual?.IsVerified.Should().BeTrue();
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Confirmed_throw_exception_when_customer_not_found(int invalidCustomerId)
    {
        var actual =()=>_sut.Confirmed(invalidCustomerId);
        
        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Customer>().Should().BeEmpty();
    }
    
    [Fact]
    public void Update_update_a_customer_properly()
    {
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder()
            .WithFirstName("Ali")
            .WithLastName("Amiri")
            .WithNationalCode("2280493291")
            .WithPhoneNumber("09177300987")
            .WithEmail("")
            .Build();
        Save(customer2);
        
        var dto = new UpdateCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = "09177329867",
            Email = "Rezahosseini@gmail.com",
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        _sut.Update(customer2.Id,dto);
        
        var actual = ReadContext.Set<Customer>().FirstOrDefault(c => c.Id == customer2.Id);
        actual.Should().BeEquivalentTo(dto);
    }
    [Theory]
    [InlineData("09171111111")]
    [InlineData("09177328978")]
    public void Update_update_a_customer_when_phone_number_not_changed_properly(string duplicatePhoneNumber)
    {
        var customer = new CustomerBuilder().WithPhoneNumber(duplicatePhoneNumber).Build();
        Save(customer);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        
        var dto = new UpdateCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = duplicatePhoneNumber,
            Email = "Rezahosseini@gmail.com",
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        _sut.Update(customer.Id,dto);
        
        ReadContext.Set<Customer>().Should().HaveCount(2)
            .And.ContainSingle(c => c.PhoneNumber == dto.PhoneNumber &&
                                    c.FirstName == dto.FirstName &&
                                    c.LastName == dto.LastName &&
                                    c.Email == dto.Email &&
                                    c.JobType == dto.JobType &&
                                    c.MonthlyIncome == dto.MonthlyIncome &&
                                    c.Assets == dto.Assets &&
                                    c.Id == customer.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Update_throw_exception_when_customer_is_not_found(int invalidCustomerId)
    {
        var dto = new UpdateCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = "09177329867",
            Email = "Rezahosseini@gmail.com",
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        var actual = () => _sut.Update(invalidCustomerId,dto);
        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Customer>().Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("09171111111")]
    [InlineData("09177328978")]
    public void Update_throw_exception_when_customer_phone_number_is_duplicate(string duplicatePhoneNumber)
    {
        var customer = new CustomerBuilder().WithPhoneNumber(duplicatePhoneNumber).Build();
        Save(customer);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        
        var dto = new UpdateCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = duplicatePhoneNumber,
            Email = "Rezahosseini@gmail.com",
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        var actual =()=> _sut.Update(customer2.Id,dto);
        actual.Should().ThrowExactly<PhoneNumberDuplicatedException>();
        ReadContext.Set<Customer>().Should().HaveCount(2)
            .And.ContainSingle(c => c.PhoneNumber == duplicatePhoneNumber);
    }

    [Fact]
    public void UpdateByAdmin_update_a_customer_properly()
    {
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder()
            .WithFirstName("Ali")
            .WithLastName("Amiri")
            .WithNationalCode("2280493291")
            .WithPhoneNumber("09177300987")
            .WithEmail("")
            .Build();
        Save(customer2);
        
        var dto = new UpdateByAdminCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = "09177329867",
            NationalCode = "2280971390",
            Email = "Rezahosseini@gmail.com",
            Documents = "www.uplod.ir/madrak1.jpg",
            IsVerified = true,
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        _sut.UpdateByAdmin(customer2.Id,dto);
        
        var actual = ReadContext.Set<Customer>().FirstOrDefault(c => c.Id == customer2.Id);
        actual.Should().BeEquivalentTo(dto);
    }
    [Theory]
    [InlineData("09171111111")]
    [InlineData("09177328978")]
    public void UpdateByAdmin_update_a_customer_when_phone_number_not_changed_properly(string duplicatePhoneNumber)
    {
        var customer = new CustomerBuilder().WithPhoneNumber(duplicatePhoneNumber).Build();
        Save(customer);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        
        var dto = new UpdateByAdminCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = duplicatePhoneNumber,
            NationalCode = "2280971390",
            Email = "Rezahosseini@gmail.com",
            Documents = "www.uplod.ir/madrak1.jpg",
            IsVerified = true,
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        _sut.UpdateByAdmin(customer.Id,dto);
        
        ReadContext.Set<Customer>().Should().HaveCount(2)
            .And.ContainSingle(c => c.PhoneNumber == dto.PhoneNumber &&
                                    c.FirstName == dto.FirstName &&
                                    c.LastName == dto.LastName &&
                                    c.NationalCode == dto.NationalCode &&
                                    c.Email == dto.Email &&
                                    c.Documents == dto.Documents &&
                                    c.IsVerified == dto.IsVerified &&
                                    c.JobType == dto.JobType &&
                                    c.MonthlyIncome == dto.MonthlyIncome &&
                                    c.Assets == dto.Assets &&
                                    c.Id == customer.Id);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void UpdateByAdmin_throw_exception_when_customer_is_not_found(int invalidCustomerId)
    {
        var dto = new UpdateByAdminCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = "09176711365",
            NationalCode = "2280971390",
            Email = "Rezahosseini@gmail.com",
            Documents = "www.uplod.ir/madrak1.jpg",
            IsVerified = true,
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        var actual = () => _sut.UpdateByAdmin(invalidCustomerId,dto);
        actual.Should().ThrowExactly<CustomerNotFoundException>();
        ReadContext.Set<Customer>().Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("09171111111")]
    [InlineData("09177328978")]
    public void UpdateByAdmin_throw_exception_when_customer_phone_number_is_duplicate(string duplicatePhoneNumber)
    {
        var customer = new CustomerBuilder().WithPhoneNumber(duplicatePhoneNumber).Build();
        Save(customer);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        
        var dto = new UpdateByAdminCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = duplicatePhoneNumber,
            NationalCode = "2280971390",
            Email = "Rezahosseini@gmail.com",
            Documents = "www.uplod.ir/madrak1.jpg",
            IsVerified = true,
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        var actual =()=> _sut.UpdateByAdmin(customer2.Id,dto);
        actual.Should().ThrowExactly<PhoneNumberDuplicatedException>();
        ReadContext.Set<Customer>().Should().HaveCount(2)
            .And.ContainSingle(c => c.PhoneNumber == duplicatePhoneNumber);
    }
    [Theory]
    [InlineData("2280459159")]
    [InlineData("1111111111")]
    public void UpdateByAdmin_throw_exception_when_customer_national_code_is_duplicate(string duplicateNationalCode)
    {
        var customer = new CustomerBuilder().WithNationalCode(duplicateNationalCode).Build();
        Save(customer);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        
        var dto = new UpdateByAdminCustomerDto
        {
            FirstName = "Reza",
            LastName = "Hosseini",
            PhoneNumber = "09177329867",
            NationalCode = duplicateNationalCode,
            Email = "Rezahosseini@gmail.com",
            Documents = "www.uplod.ir/madrak1.jpg",
            IsVerified = true,
            JobType = JobType.Employee,
            MonthlyIncome = 10000000m,
            Assets = 50000000m
        };
        
        var actual =()=> _sut.UpdateByAdmin(customer2.Id,dto);
        actual.Should().ThrowExactly<NationalCodeDuplicatedException>();
    }
    
}