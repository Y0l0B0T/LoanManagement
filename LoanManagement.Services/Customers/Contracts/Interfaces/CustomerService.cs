using LoanManagement.Services.Customers.Contracts.DTOs;

namespace LoanManagement.Services.Customers.Contracts.Interfaces;

public interface CustomerService
{
    void Add(AddCustomerDto dto);
    void AddDocuments(int customerId, AddDocumentsDto dto);
    void Confirmed(int customerId);
    void AddFinancialInfo(int customerId,AddFinancialInfoDto dto);
    void Update(int customerId,UpdateCustomerDto dto);
    void UpdateByAdmin(int customerId, UpdateByAdminCustomerDto dto);
}