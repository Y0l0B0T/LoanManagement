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