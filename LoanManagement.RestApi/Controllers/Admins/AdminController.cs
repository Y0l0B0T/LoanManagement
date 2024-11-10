namespace LoanManagement.RestApi.Controllers.Admins;
[ApiController]
[Route("api/v1/admin")]
public class AdminController(AdminService adminService
, AdminQuery adminQuery) : ControllerBase
{
    [HttpPost]
    public void Add([FromBody] AddAdminDto dto)
    {
        adminService.Add(dto);
    }

    [HttpDelete("{adminId}/delete")]
    public void Delete(int adminId)
    {
        adminService.Delete(adminId);
    }

    [HttpGet("getall")]
    public HashSet<GetAllAdminsDto> GetAll()
    {
        return adminQuery.GetAll();
    }

    [HttpGet("{adminId}/getbyid")]
    public Admin? GetById(int adminId)
    {
        return adminQuery.GetById(adminId);
    }
}