using LoanManagement.Services.installments.Contracts.DTOs;

namespace LoanManagement.RestApi.Controllers.installments;
[ApiController]
[Route("api/v1/installment")]
public class InstallmentController(InstallmentService installmentService,
    InstallmentQuery installmentQuery) : ControllerBase
{
    [HttpPatch("{installmentId}/installment")]
    public void PayInstallment(int installmentId,
        [FromBody] PayInstallmentDto dto)
    {
        installmentService.PayInstallment(installmentId, dto);
    }

    [HttpGet("{loanId}/getall")]
    public HashSet<GetAllInstallmentsOfLoanDto> GetAllInstallmentsOfLoan(int loanId)
    {
        return installmentQuery.GetAllInstallmentsOfLoan(loanId);
    }
}