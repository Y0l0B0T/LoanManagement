using LoanManagement.Entities.Admins;
using LoanManagement.Service.Unit.Tests.Admins;
using LoanManagement.Services.Admins.Contracts.Interfaces;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.UnitOfWorks;

namespace LoanManagement.Services.Admins;

public class AdminAppService(
    AdminRepository adminRepository,
    UnitOfWork unitOfWork) : AdminService
{
    public void Add(AddAdminDto dto)
    {
        var admin = new Admin()
        {
            Name = dto.Name,
        };
        adminRepository.Add(admin);
        unitOfWork.Save();
    }

    public void Delete(int id)
    {
        var admin = adminRepository.Find(id)
                    ?? throw new AdminNotFoundException();
        adminRepository.Delete(admin);
        unitOfWork.Save();
    }
}