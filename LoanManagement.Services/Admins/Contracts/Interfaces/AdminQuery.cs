using LoanManagement.Entities.Admins;
using LoanManagement.Service.Unit.Tests.Admins;

namespace LoanManagement.Services.Admins.Contracts.Interfaces;

public interface AdminQuery
{
    HashSet<GetAllAdminsDto> GetAll();
    Admin? GetById(int adminId);
}