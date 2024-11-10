using LoanManagement.Entities.Loans;

namespace LoanManagement.RestApi.Controllers.Loans;

[ApiController]
[Route("api/v1/loan")]
public class AdminLoanController(
    LoanService loanService,
    LoanQuery loanQuery) : ControllerBase
{
    [HttpPost("admin/add")]
    public void Add([FromBody] AddLoanDto dto)
    {
        loanService.Add(dto);
    }

    [HttpPatch("{loanId}/confirm")]
    public void ConfirmLoan(int adminId,int loanId)
    {
        loanService.ConfirmLoan(adminId, loanId);
    }

    [HttpPatch("{loanId}/reject")]
    public void RejectLoan(int adminId,int loanId)
    {
        loanService.RejectLoan(adminId, loanId);
    }

    [HttpPatch("{loanId}/pay")]
    public void PayLoan(int adminId,int loanId)
    {
        loanService.PayLoan(adminId, loanId);
    }

    [HttpPatch("{loanId}/delayinpay")]
    public void DelayInPayLoan(int loanId)
    {
        loanService.DelayInPayLoan(loanId);
    }

    [HttpPatch("{loanId}/closed")]
    public void ClosedLoan(int loanId)
    {
        loanService.ClosedLoan(loanId);
    }

    [HttpGet("{loanId}/getbyid")]
    public Loan? GetById(int loanId)
    {
        return loanQuery.GetById(loanId);
    }
    [HttpGet("getall")]
    public HashSet<GetAllLoansDto> GetAll()
    {
        return loanQuery.GetAll();
    }
    [HttpGet("getallreview")]
    public HashSet<GetAllLoansDto> GetAllInReview()
    {
        return loanQuery.GetAllInReview();
    }

    [HttpGet("getallapproved")]
    public HashSet<GetAllLoansDto> GetAllInApproved()
    {
        return loanQuery.GetAllInApproved();
    }

    [HttpGet("getallrejected")]
    public HashSet<GetAllLoansDto> GetAllInRejected()
    {
        return loanQuery.GetAllInRejected();
    }

    [HttpGet("getallpaying")]
    public HashSet<GetAllLoansDto> GetAllInPaying()
    {
        return loanQuery.GetAllInPaying();
    }

    [HttpGet("getallactive")]
    public HashSet<GetAllLoansDto> GetAllActiveLoans()
    {
        return loanQuery.GetAllActiveLoans();
    }

    [HttpGet("getallclosed")]
    public HashSet<GetAllLoansDto> GetAllInClosed()
    {
        return loanQuery.GetAllInClosed();
    }

    [HttpGet("{loadId}/getpendinginstallmentsloan")]
    public HashSet<GetPendingInstallmentsByLoanIdDto> GetPendingInstallmentsByLoanId(int loadId)
    {
        return loanQuery.GetPendingInstallmentsByLoanId(loadId);
    }
}