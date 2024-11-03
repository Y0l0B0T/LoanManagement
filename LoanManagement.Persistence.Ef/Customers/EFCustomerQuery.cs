using LoanManagement.Entities.Customers;
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
            MonthlyIncome = _.MonthlyIncome,
            JobType = _.JobType,
            Assets = _.Assets,
            IsVerified = _.IsVerified,
        }).ToHashSet();
    }
}