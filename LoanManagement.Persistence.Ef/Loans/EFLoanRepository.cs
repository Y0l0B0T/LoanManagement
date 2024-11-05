using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

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
                LoanType = _.LoanType,
                Status = _.Status,
                Installments = _.Installments
            }).ToHashSet();
    }

    public Loan Find(int loanId)
    {
        return context.Set<Loan>().Include(l=>l.LoanDefinition).FirstOrDefault(_ => _.Id == loanId);
    }
    
    public Loan FindInstallments(int loanId)
    {
        return context.Set<Loan>().Include(_=>_.Installments).FirstOrDefault(_ => _.Id == loanId);
    }
    public void Update(Loan loan)
    {
        context.Set<Loan>().Update(loan);
    }

    public void AddInstallmens(Loan loan)
    {
        context.Set<Installment>().AddRange(loan.Installments);
    }
}