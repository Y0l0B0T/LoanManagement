using LoanManagement.Entities.installments;
using LoanManagement.Services.installments.Contracts.DTOs;
using LoanManagement.Services.installments.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.installments;

public class EFInstallmentQuery(EfDataContext context) : InstallmentQuery
{
    public Installment? GetById(int installmentId)
    {
        return context.Set<Installment>().FirstOrDefault(x => x.Id == installmentId);
    }

    public HashSet<GetAllInstallmentsDto> GetAll()
    {
        return context.Set<Installment>().Select(_ => new GetAllInstallmentsDto
        {
            Id = _.Id,
            LoanId = _.LoanId,
            Status = _.Status,
            DueTime = _.DueTime,
            PaymentTime =  _.PaymentTime,
        }).ToHashSet();
    }

    public HashSet<GetAllInstallmentsOfLoanDto> GetAllInstallmentsOfLoan(int loanId)
    {
        return context.Set<Installment>()
            .Where(_ => _.LoanId == loanId)
            .Select(_ => new GetAllInstallmentsOfLoanDto
            {
                Id = _.Id,
                Status = _.Status,
                DueTime = _.DueTime,
                PaymentTime = _.PaymentTime,
            }).ToHashSet();
    }
}