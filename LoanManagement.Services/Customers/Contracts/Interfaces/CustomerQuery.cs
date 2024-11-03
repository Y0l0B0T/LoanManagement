using LoanManagement.Entities.Customers;
using LoanManagement.Services.Customers.Contracts.DTOs;

namespace LoanManagement.Services.Customers.Contracts.Interfaces;

public interface CustomerQuery
{
    Customer? GetById(int customerId);
    HashSet<GetAllCustomersDto> GetAll();
}