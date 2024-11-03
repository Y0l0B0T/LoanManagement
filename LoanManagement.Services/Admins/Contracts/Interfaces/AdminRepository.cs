using LoanManagement.Entities.Admins;

namespace LoanManagement.Services.Admins.Contracts.Interfaces;

public interface AdminRepository
{
    void Add(Admin admin);
    Admin Find(int id);
    void Delete(Admin admin);
}