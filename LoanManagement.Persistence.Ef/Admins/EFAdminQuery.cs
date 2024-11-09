namespace LoanManagement.Persistence.Ef.Admins;

public class EFAdminQuery(EfDataContext context) : AdminQuery
{
    public HashSet<GetAllAdminsDto> GetAll()
    {
        return context.Set<Admin>().Select(_ => new GetAllAdminsDto
        {
            Id = _.Id,
            Name = _.Name,
        }).ToHashSet();
    }

    public Admin? GetById(int adminId)
    {
        return context.Set<Admin>().FirstOrDefault(x => x.Id == adminId);
    }
}