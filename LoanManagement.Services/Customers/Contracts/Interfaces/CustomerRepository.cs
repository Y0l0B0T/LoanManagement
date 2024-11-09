namespace LoanManagement.Services.Customers.Contracts.Interfaces;

public interface CustomerRepository
{
    void Add(Customer customer);
    bool IsDuplicateNationalCode(string nationalCode);
    bool IsDuplicatePhoneNumber(string phoneNumber);
    Customer Find(int id);
    void Update(Customer customer);
}