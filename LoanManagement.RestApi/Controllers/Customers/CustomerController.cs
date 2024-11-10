namespace LoanManagement.RestApi.Controllers.Customers;
[Route("api/v1/customer")]
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

    [HttpPatch("{customerId}/adddocuments")]
    public void AddDocuments(int customerId, [FromBody] AddDocumentsDto dto)
    {
        customerService.AddDocuments(customerId, dto);
    }

    [HttpPatch("{customerId}/addfinancialinfo")]
    public void AddFinancialInfo(int customerId, [FromBody] AddFinancialInfoDto dto)
    {
        customerService.AddFinancialInfo(customerId, dto);
    }
    
    [HttpPut("{customerId}/update")]
    public void Update(int customerId, [FromBody] UpdateCustomerDto dto)
    {
        customerService.Update(customerId, dto);
    }
    [HttpGet("{nationalCode}/getbycode")]
    public Customer? GetByNationalCode(string nationalCode)
    {
        return customerQuery.GetByNationalCode(nationalCode);
    }
}