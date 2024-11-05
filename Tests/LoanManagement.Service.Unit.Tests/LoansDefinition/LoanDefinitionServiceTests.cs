using FluentAssertions;
using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.LoansDefinition.Contracts.DTOs;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;
using LoanManagement.TestTools.Admins;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.LoansDefinition;

public class LoanDefinitionServiceTests : BusinessIntegrationTest
{
    private readonly LoanDefinitionService _sut;

    public LoanDefinitionServiceTests()
    {
        
        _sut = LoanDefinitionServiceFactory.Generate(SetupContext);
    }

    [Theory]
    [InlineData(40000000, 9)]
    [InlineData(60000000, 12)]
    [InlineData(80000000, 18)]
    public void Add_add_a_loan_definition_properly(decimal loanAmount, int installmentsCount)
    {
        var admin = AdminFactory.Create();
        Save(admin);
        
        var dto = new AddLoanDefinitionDto
        {
            LoanAmount = loanAmount,
            InstallmentsCount = installmentsCount
        };
        
        _sut.Add(admin.Id, dto);

        var actual = ReadContext.Set<LoanDefinition>().Single();
        actual.Should().NotBeNull();
        actual.LoanAmount.Should().Be(dto.LoanAmount);
        actual.InstallmentsCount.Should().Be(dto.InstallmentsCount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(1)]
    public void Add_throw_exception_when_admin_not_found(int invalidAdminId)
    {
        var dto = new AddLoanDefinitionDto
        {
            LoanAmount = 100000m,
            InstallmentsCount = 12,
        };
        
        var actual =()=> _sut.Add(invalidAdminId, dto);

        actual.Should().ThrowExactly<AdminNotFoundException>();
        ReadContext.Set<LoanDefinition>().Should().BeEmpty();

    }
}