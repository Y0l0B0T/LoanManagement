namespace LoanManagement.Services.LoansDefinition.Contracts.DTOs;

public class AddLoanDefinitionDto
{
    [Range(1, double.MaxValue)]public decimal LoanAmount { get; set; }
    [Range(1,60)]public int InstallmentsCount { get; set; }
}