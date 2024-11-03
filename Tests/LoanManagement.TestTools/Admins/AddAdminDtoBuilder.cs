using LoanManagement.Service.Unit.Tests.Admins;

namespace LoanManagement.TestTools.Admins;

public class AddAdminDtoBuilder
{
    private readonly AddAdminDto _addAdminDto;

    public AddAdminDtoBuilder()
    {
        _addAdminDto = new AddAdminDto()
        {
            Name = "Admin",
        };
    }

    public AddAdminDtoBuilder WithName(string name)
    {
        _addAdminDto.Name = name;
        return this;
    }

    public AddAdminDto Build()
    {
        return _addAdminDto;
    }
}