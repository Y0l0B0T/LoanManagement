using LoanManagement.Entities.Customers;
using LoanManagement.Services.Customers.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.Customers;

public class EFCustomerRepository(EfDataContext context) : CustomerRepository
{
    public void Add(Customer customer)
    {
        context.Set<Customer>().Add(customer);
    }

    public bool IsDuplicateNationalCode(string nationalCode)
    {
        return context.Set<Customer>().Any(_ => _.NationalCode == nationalCode);
    }
    public bool IsDuplicatePhoneNumber(string phoneNumber)
    {
        return context.Set<Customer>().Any(_ => _.PhoneNumber == phoneNumber);
    }

    public Customer Find(int id)
    {
        return context.Set<Customer>().FirstOrDefault(_ => _.Id == id);
    }

    public void Update(Customer customer)
    {
        context.Set<Customer>().Update(customer);
    }
}