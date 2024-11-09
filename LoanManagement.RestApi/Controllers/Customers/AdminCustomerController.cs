namespace LoanManagement.RestApi.Controllers.Customers;

[ApiController]
[Route("api/[controller]")]
public class AdminCustomersController(CustomerService customerService,
    CustomerQuery customerQuery) : ControllerBase
{
    [HttpPatch("{id}/confirm")]
    public void ConfirmDocument(int adminId, [FromRoute] int customerId)
    {
        customerService.ConfirmDocument(adminId, customerId);
    }
    
    [HttpPatch("{id}/reject")]
    public void RejectDocument(int adminId, [FromRoute] int customerId)
    {
        customerService.RejectDocument(adminId, customerId);
    }

    [HttpPut("{id}/update")]
    public void UpdateByAdmin(int adminId,[FromRoute] int customerId, [FromBody] UpdateByAdminCustomerDto dto)
    {
        customerService.UpdateByAdmin(adminId, customerId, dto);
    }
    
    [HttpGet]
    public HashSet<GetAllCustomersDto> GetAll()
    {
        return customerQuery.GetAll();
    }
    
    [HttpGet("{id}")]
    public Customer? GetById([FromRoute] int customerId)
    {
        return customerQuery.GetById(customerId);
    }
}
