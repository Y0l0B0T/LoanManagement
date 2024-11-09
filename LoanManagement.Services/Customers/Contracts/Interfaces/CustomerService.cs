namespace LoanManagement.Services.Customers.Contracts.Interfaces;

public interface CustomerService
{
    void Add(AddCustomerDto dto);
    void AddDocuments(int customerId, AddDocumentsDto dto);
    void ConfirmDocument(int adminId,int customerId);
    void RejectDocument(int adminId, int customerId);
    void AddFinancialInfo(int customerId,AddFinancialInfoDto dto);
    void Update(int customerId,UpdateCustomerDto dto);
    void UpdateByAdmin(int adminId,int customerId, UpdateByAdminCustomerDto dto);
}