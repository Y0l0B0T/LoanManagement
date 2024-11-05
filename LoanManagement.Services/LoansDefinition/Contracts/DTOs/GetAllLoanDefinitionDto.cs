namespace LoanManagement.Services.LoansDefinition.Contracts.DTOs;

public class GetAllLoanDefinitionDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int InstallmentsCount { get; set; }
    public decimal InstallmentAmount { get; set; }
    public decimal BasePaymentAmount { get; set; }
    public decimal MonthlyInterestAmount { get; set; }
    public decimal MonthlyPenaltyAmount { get; set; }
}