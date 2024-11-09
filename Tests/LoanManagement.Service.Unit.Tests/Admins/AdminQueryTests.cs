namespace LoanManagement.Service.Unit.Tests.Admins;

public class AdminQueryTests : BusinessIntegrationTest
{
    private readonly AdminQuery _sut;

    public AdminQueryTests()
    {
        _sut = new EFAdminQuery(ReadContext);
    }

    [Fact]
    public void GetAll_get_all_admins_properly()
    {
        var admin1 = new AdminBuilder().WithName("Admin1").Build();
        Save(admin1);
        var admin2 = new AdminBuilder().WithName("Admin2").Build();
        Save(admin2);

        var actual = _sut.GetAll();

        var expected = ReadContext.Set<Admin>().ToHashSet();
        
        actual.Should().BeEquivalentTo(expected)
            .And.ContainSingle(a=>a.Id == admin1.Id && a.Name == admin1.Name)
            .And.ContainSingle(a=>a.Id == admin2.Id && a.Name == admin2.Name);

    }

    [Fact]
    public void GetById_get_an_admin_by_id_properly()
    {
        var admin = new AdminBuilder().WithName("Admin1").Build();
        Save(admin);
        
        var actual = _sut.GetById(admin.Id);
        
        actual.Should().BeEquivalentTo(admin);
        actual!.Id.Should().Be(admin.Id);
        actual.Name.Should().Be(admin.Name);
    }
}