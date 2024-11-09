namespace LoanManagement.Services.Customers.Contracts.Interfaces;

public interface CustomerQuery
{
    Customer? GetById(int customerId);
    HashSet<GetAllCustomersDto> GetAll();
    HashSet<GetAllCustomersPendingForConfirmation> GetAllCustomersPendingForConfirmation();
    Customer? GetByNationalCode(string nationalCode);
    HashSet<GetLoansByNationalCodeDto> GetLoansByNationalCode(string nationalCode);
}