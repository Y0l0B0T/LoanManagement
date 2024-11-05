using LoanManagement.Entities.Customers;
using LoanManagement.Entities.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.Loans;

public class EFLoanRepository(EfDataContext context) : LoanRepository
{
    public void Add(Loan loan)
    {
        context.Set<Loan>().Add(loan);
    }

    public bool IsDuplicate(int customerId, int loanDefinitionId)
    {
        return context.Set<Loan>().Any
            (l => l.CustomerId == customerId &&
                  l.LoanDefinitionId == loanDefinitionId);
    }


    public HashSet<GetCustomerLoansByIdDto> GetCustomerLoansById(int customerId)
    {
        return context.Set<Customer>()
            .FirstOrDefault(_ => _.Id == customerId).MyLoans.Select(_ => new GetCustomerLoansByIdDto
            {
                Status = _.Status,
                Installments = _.Installments
            }).ToHashSet();
    }

    public Loan Find(int loanId)
    {
        return context.Set<Loan>().FirstOrDefault(_ => _.Id == loanId);
    }

    public void Update(Loan loan)
    {
        context.Set<Loan>().Update(loan);
    }
}