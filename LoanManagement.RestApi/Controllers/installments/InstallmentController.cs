using LoanManagement.Services.installments.Contracts.DTOs;

namespace LoanManagement.RestApi.Controllers.installments;
[ApiController]
[Route("api/[controller]")]
public class InstallmentController(InstallmentService installmentService,
    InstallmentQuery installmentQuery) : ControllerBase
{
    [HttpPatch("{id}/installment")]
    public void PayInstallment([FromRoute] int installmentId,
        [FromBody] PayInstallmentDto dto)
    {
        installmentService.PayInstallment(installmentId, dto);
    }

    [HttpGet("{id}")]
    public HashSet<GetAllInstallmentsOfLoanDto> GetAllInstallmentsOfLoan([FromRoute]int loanId)
    {
        return installmentQuery.GetAllInstallmentsOfLoan(loanId);
    }
}