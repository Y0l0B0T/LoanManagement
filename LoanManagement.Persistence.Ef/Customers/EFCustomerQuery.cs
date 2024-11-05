using LoanManagement.Entities.Customers;
using LoanManagement.Entities.Loans;
using LoanManagement.Services.Customers.Contracts.DTOs;
using LoanManagement.Services.Customers.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.Customers;

public class EFCustomerQuery(EfDataContext context) : CustomerQuery
{
    public Customer? GetById(int customerId)
    {
        return context.Set<Customer>().FirstOrDefault(x => x.Id == customerId);
    }

    public HashSet<GetAllCustomersDto> GetAll()
    {
        return context.Set<Customer>().Select(_ => new GetAllCustomersDto
        {
            Id = _.Id,
            FirstName = _.FirstName,
            LastName = _.LastName,
            NationalCode = _.NationalCode,
            PhoneNumber = _.PhoneNumber,
            Email = _.Email,
            Documents = _.Documents,
            MonthlyIncome = _.MonthlyIncome,
            JobType = _.JobType,
            Assets = _.Assets,
            IsVerified = _.IsVerified,
        }).ToHashSet();
    }

    public HashSet<GetAllCustomersPendingForConfirmation> GetAllCustomersPendingForConfirmation()
    {
        return context.Set<Customer>()
            .Where(c => !c.IsVerified && c.Documents != null)
            .Select(_=>new GetAllCustomersPendingForConfirmation
        {
            Id = _.Id,
            FirstName = _.FirstName,
            LastName = _.LastName,
            NationalCode = _.NationalCode,
            PhoneNumber =  _.PhoneNumber,
            Email = _.Email,
            Documents = _.Documents,
        }).ToHashSet();
    }

    public Customer? GetByNationalCode(string nationalCode)
    {
        return context.Set<Customer>().FirstOrDefault(x => x.NationalCode == nationalCode);
    }
    public HashSet<GetLoansByNationalCodeDto> GetLoansByNationalCode(string nationalCode)
    {
        var query = from customer in context.Set<Customer>()
            join loan in context.Set<Loan>()
                on customer.Id equals loan.CustomerId
            where customer.NationalCode == nationalCode
            select new GetLoansByNationalCodeDto
            {
                Status = loan.Status,
                LoanType = loan.LoanType,
                InstallmentsCount = loan.Installments.Count
            };
        return query.ToHashSet();
    }
}