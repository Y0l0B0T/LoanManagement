using LoanManagement.Entities.Admins;
using LoanManagement.Services.Admins.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.Admins;

public class EFAdminRepository(EfDataContext context) : AdminRepository
{
    public void Add(Admin admin)
    {
       context.Set<Admin>().Add(admin);
    }

    public Admin? Find(int id)
    {
        return context.Set<Admin>().FirstOrDefault(x => x.Id == id);
    }

    public void Delete(Admin admin)
    {
        context.Set<Admin>().Remove(admin);
    }
}