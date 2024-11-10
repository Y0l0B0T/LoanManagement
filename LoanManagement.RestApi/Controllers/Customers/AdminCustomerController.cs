namespace LoanManagement.RestApi.Controllers.Customers;

[ApiController]
[Route("api/v1/customer")]
public class AdminCustomersController(CustomerService customerService,
    CustomerQuery customerQuery) : ControllerBase
{
    [HttpPatch("{customerId}/confirm")]
    public void ConfirmDocument(int adminId, int customerId)
    {
        customerService.ConfirmDocument(adminId, customerId);
    }
    
    [HttpPatch("{customerId}/reject")]
    public void RejectDocument(int adminId, int customerId)
    {
        customerService.RejectDocument(adminId, customerId);
    }

    [HttpPut("admin/{customerId}/update")]
    public void UpdateByAdmin(int adminId,int customerId, [FromBody] UpdateByAdminCustomerDto dto)
    {
        customerService.UpdateByAdmin(adminId, customerId, dto);
    }
    
    [HttpGet("getall")]
    public HashSet<GetAllCustomersDto> GetAll()
    {
        return customerQuery.GetAll();
    }
    
    [HttpGet("{customerId}/getbyid")]
    public Customer? GetById(int customerId)
    {
        return customerQuery.GetById(customerId);
    }
}
