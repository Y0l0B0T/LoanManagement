namespace LoanManagement.RestApi.Controllers.Loans;

[ApiController]
[Route("api/[controller]")]
public class LoanController(LoanService loanService,
    LoanQuery loanQuery) : ControllerBase
{
    [HttpPost("add")]
    public void Add([FromBody] AddLoanDto dto)
    {
        loanService.Add(dto);
    }
    [HttpGet("{id}")]
    public HashSet<GetPendingInstallmentsByLoanIdDto> GetPendingInstallmentsByLoanId([FromRoute] int loadId)
    {
        return loanQuery.GetPendingInstallmentsByLoanId(loadId);
    }
}