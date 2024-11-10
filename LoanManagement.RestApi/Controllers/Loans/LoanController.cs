namespace LoanManagement.RestApi.Controllers.Loans;

[ApiController]
[Route("api/v1/loan")]
public class LoanController(LoanService loanService,
    LoanQuery loanQuery) : ControllerBase
{
    [HttpPost("add")]
    public void Add([FromBody] AddLoanDto dto)
    {
        loanService.Add(dto);
    }
    [HttpGet("{loadId}/getinstallmentsloan")]
    public HashSet<GetPendingInstallmentsByLoanIdDto> GetPendingInstallmentsByLoanId(int loadId)
    {
        return loanQuery.GetPendingInstallmentsByLoanId(loadId);
    }
}