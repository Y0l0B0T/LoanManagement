using FluentAssertions;
using LoanManagement.Entities.Admins;
using LoanManagement.Persistence.Ef.Admins;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.Admins;
using LoanManagement.Services.Admins.Contracts.Interfaces;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.TestTools.Admins;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.Admins;

public class AdminServiceTests : BusinessIntegrationTest
{
    private readonly AdminService _sut;

    public AdminServiceTests()
    {
        _sut = AdminServiceFactory.Generate(SetupContext);
    }

    [Fact]
    public void Add_add_an_admin_properly()
    {
        var dto = new AddAdminDto()
        {
            Name = "Admin",
        };

        _sut.Add(dto);

        var actual = ReadContext.Set<Admin>().Single();
        actual.Should().BeEquivalentTo(new Admin()
        {
            Name = dto.Name,
        }, c => c.Excluding(a => a.Id));
    }

    [Fact]
    public void Delete_delete_an_admin_properly()
    {
        var admin = new AdminBuilder().WithName("admin").Build();
        Save(admin);

        _sut.Delete(admin.Id);

        var actual = ReadContext.Set<Admin>().ToHashSet();
        actual.Should().NotContain(a => a.Id == admin.Id);
    }

    [Theory]
    [InlineData(-1)]
    public void Delete_throw_exception_when_admin_not_found(int invalidAdminId)
    {
        var actual = () => _sut.Delete(invalidAdminId);

        actual.Should().Throw<AdminNotFoundException>();
        ReadContext.Set<Admin>().Should().BeEmpty();
    }
}