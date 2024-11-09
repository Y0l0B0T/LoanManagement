namespace LoanManagement.RestApi.Controllers.Admins;
[ApiController]
[Route("[controller]")]
public class AdminController(AdminService adminService
, AdminQuery adminQuery) : ControllerBase
{
    [HttpPost]
    public void Add([FromBody] AddAdminDto dto)
    {
        adminService.Add(dto);
    }

    [HttpDelete("id")]
    public void Delete([FromRoute] int adminId)
    {
        adminService.Delete(adminId);
    }

    [HttpGet]
    public HashSet<GetAllAdminsDto> GetAll()
    {
        return adminQuery.GetAll();
    }

    [HttpGet("{id}")]
    public Admin? GetById(int adminId)
    {
        return adminQuery.GetById(adminId);
    }
}