namespace LoanManagement.Services.UnitOfWorks;

public interface UnitOfWork
{
    Task SaveAsync();
    void Save();
    Task Begin();
    Task Rollback();
    Task Commit();
}