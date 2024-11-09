namespace LoanManagement.Services.Admins.Contracts.Interfaces;

public interface AdminQuery
{
    HashSet<GetAllAdminsDto> GetAll();
    Admin? GetById(int adminId);
}