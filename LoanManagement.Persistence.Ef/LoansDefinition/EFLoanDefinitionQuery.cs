namespace LoanManagement.Persistence.Ef.LoansDefinition;

public class EFLoanDefinitionQuery(EfDataContext context) : LoanDefinitionQuery
{
    public LoanDefinition? GetById(int id)
    {
        return context.Set<LoanDefinition>().FirstOrDefault(x => x.Id == id);
    }
    
    public HashSet<GetAllLoanDefinitionDto> GetAll()
    {
        return context.Set<LoanDefinition>().Select(_ => new GetAllLoanDefinitionDto
        {
            Id = _.Id,
            Name = _.Name,
            MonthlyPenaltyAmount = _.MonthlyPenaltyAmount,
            InstallmentAmount = _.InstallmentAmount,
            InstallmentsCount =  _.InstallmentsCount,
            InterestRate = _.InterestRate,
            BasePaymentAmount = _.BasePaymentAmount,
            MonthlyInterestAmount = _.MonthlyInterestAmount,
            LoanAmount = _.LoanAmount,
        }).ToHashSet();
    }
}