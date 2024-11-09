using LoanManagement.Entities.Loans;

namespace LoanManagement.RestApi.Controllers.Loans;

[ApiController]
[Route("api/[controller]")]
public class AdminLoanController(
    LoanService loanService,
    LoanQuery loanQuery) : ControllerBase
{
    [HttpPost("add")]
    public void Add([FromBody] AddLoanDto dto)
    {
        loanService.Add(dto);
    }

    [HttpPatch("{id}/confirm")]
    public void ConfirmLoan(int adminId,[FromRoute] int loanId)
    {
        loanService.ConfirmLoan(adminId, loanId);
    }

    [HttpPatch("{id}/reject")]
    public void RejectLoan(int adminId,[FromRoute] int loanId)
    {
        loanService.RejectLoan(adminId, loanId);
    }

    [HttpPatch("{id}/pay")]
    public void PayLoan(int adminId,[FromRoute] int loanId)
    {
        loanService.PayLoan(adminId, loanId);
    }

    [HttpPatch("{id}/delayinpay")]
    public void DelayInPayLoan([FromRoute] int loanId)
    {
        loanService.DelayInPayLoan(loanId);
    }

    [HttpPatch("{id}/closed")]
    public void ClosedLoan([FromRoute] int loanId)
    {
        loanService.ClosedLoan(loanId);
    }

    [HttpGet("{id}")]
    public Loan? GetById(int loanId)
    {
        return loanQuery.GetById(loanId);
    }
    [HttpGet]
    public HashSet<GetAllLoansDto> GetAll()
    {
        return loanQuery.GetAll();
    }
    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllInReview()
    {
        return loanQuery.GetAllInReview();
    }

    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllInApproved()
    {
        return loanQuery.GetAllInApproved();
    }

    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllInRejected()
    {
        return loanQuery.GetAllInRejected();
    }

    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllInPaying()
    {
        return loanQuery.GetAllInPaying();
    }

    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllActiveLoans()
    {
        return loanQuery.GetAllActiveLoans();
    }

    [HttpGet]
    public HashSet<GetAllLoansDto> GetAllInClosed()
    {
        return loanQuery.GetAllInClosed();
    }

    [HttpGet("{id}")]
    public HashSet<GetPendingInstallmentsByLoanIdDto> GetPendingInstallmentsByLoanId([FromRoute] int loadId)
    {
        return loanQuery.GetPendingInstallmentsByLoanId(loadId);
    }
}