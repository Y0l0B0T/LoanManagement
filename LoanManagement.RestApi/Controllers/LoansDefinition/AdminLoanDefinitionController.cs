using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.LoansDefinition.Contracts.DTOs;

namespace LoanManagement.RestApi.Controllers.LoansDefinition;
[ApiController]
[Route("api/v1/loandefinition")]
public class AdminLoanDefinitionController
    (LoanDefinitionService lService,LoanDefinitionQuery lQuery )
    : ControllerBase
{
    [HttpPost("add")]
    public void Add(int adminId,[FromBody] AddLoanDefinitionDto dto)
    {
        lService.Add(adminId, dto);
    }

    [HttpGet("{id}/getbyid")]
    public LoanDefinition? GetById(int id)
    {
        return lQuery.GetById(id);
    }

    [HttpGet("getall")]
    public HashSet<GetAllLoanDefinitionDto> GetAll()
    {
        return lQuery.GetAll();
    }
}