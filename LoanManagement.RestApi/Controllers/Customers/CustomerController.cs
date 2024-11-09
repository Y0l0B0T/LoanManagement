namespace LoanManagement.RestApi.Controllers.Customers;
[Route("api/[controller]")]
[ApiController]
public class CusomerController(
    CustomerService customerService,
    CustomerQuery customerQuery) : ControllerBase
{
    [HttpPost("add")]
    public void Add([FromBody] AddCustomerDto dto)
    {
        customerService.Add(dto);
    }

    [HttpPatch("{id}/adddocuments")]
    public void AddDocuments(int customerId, [FromBody] AddDocumentsDto dto)
    {
        customerService.AddDocuments(customerId, dto);
    }

    [HttpPatch("{id}/addfinancialinfo")]
    public void AddFinancialInfo([FromRoute] int customerId, [FromBody] AddFinancialInfoDto dto)
    {
        customerService.AddFinancialInfo(customerId, dto);
    }
    
    [HttpPut("{id}/update")]
    public void Update([FromRoute] int customerId, [FromBody] UpdateCustomerDto dto)
    {
        customerService.Update(customerId, dto);
    }
    [HttpGet("{id}")]
    public Customer? GetByNationalCode(string nationalCode)
    {
        return customerQuery.GetByNationalCode(nationalCode);
    }
}