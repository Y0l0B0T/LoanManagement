namespace LoanManagement.Services.LoansDefinition.Contracts.DTOs;

public class AddLoanDefinitionDto
{
    public decimal LoanAmount { get; set; }
    public int InstallmentsCount { get; set; }
}