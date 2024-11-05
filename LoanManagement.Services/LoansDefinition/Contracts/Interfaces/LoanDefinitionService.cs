using LoanManagement.Services.LoansDefinition.Contracts.DTOs;

namespace LoanManagement.Services.LoansDefinition.Contracts.Interfaces;

public interface LoanDefinitionService
{
    void Add(int adminId, AddLoanDefinitionDto dto);
}