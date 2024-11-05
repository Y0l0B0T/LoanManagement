using LoanManagement.Entities.Customers;
using LoanManagement.Services.Admins.Contracts.Interfaces;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.Customers.Contracts.DTOs;
using LoanManagement.Services.Customers.Contracts.Interfaces;
using LoanManagement.Services.Customers.Exceptions;
using LoanManagement.Services.UnitOfWorks;

namespace LoanManagement.Services.Customers;

public class CustomerAppService(
    AdminRepository adminRepository,
    CustomerRepository customerRepository,
    UnitOfWork unitOfWork) : CustomerService
{
    public void Add(AddCustomerDto dto)
    {
        var isDuplicateNationalCode = customerRepository.IsDuplicateNationalCode(dto.NationalCode);
        if (isDuplicateNationalCode)
            throw new NationalCodeDuplicatedException();

        var isDuplicatePhoneNumber = customerRepository.IsDuplicatePhoneNumber(dto.PhoneNumber);
        if (isDuplicatePhoneNumber)
            throw new PhoneNumberDuplicatedException();

        var customer = new Customer
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            NationalCode = dto.NationalCode,
            PhoneNumber = dto.PhoneNumber,
            Email = dto.Email
        };
        customerRepository.Add(customer);
        unitOfWork.Save();
    }

    public void AddDocuments(int customerId, AddDocumentsDto dto)
    {
        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.Documents != null) throw new CustomerHasAlreadyDocumentsException();

        customer.Documents = dto.Documents;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }


    public void AddFinancialInfo(int customerId, AddFinancialInfoDto dto)
    {
        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.IsVerified == false) throw new CustomerIsNotVerifiedException();

        customer.JobType = dto.JobType;
        customer.MonthlyIncome = dto.MonthlyIncome;
        customer.Assets = dto.Assets;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void ConfirmDocument(int adminId, int customerId)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();

        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.Documents == null) throw new CustomerDocumentsNotFoundException();
        if (customer.IsVerified == true) throw new CustomerHasAlreadyVerifiedException();

        customer.IsVerified = true;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void RejectDocument(int adminId, int customerId)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();

        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();

        customer.Documents = null;
        customer.IsVerified = false;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void Update(int customerId, UpdateCustomerDto dto)
    {
        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();
        if (customer.PhoneNumber != dto.PhoneNumber)
        {
            var isDuplicatePhoneNumber = customerRepository.IsDuplicatePhoneNumber(dto.PhoneNumber);
            if (isDuplicatePhoneNumber)
                throw new PhoneNumberDuplicatedException();
        }

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.Email = dto.Email;
        customer.JobType = dto.JobType;
        customer.MonthlyIncome = dto.MonthlyIncome;
        customer.Assets = dto.Assets;
        customer.IsVerified = false;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }

    public void UpdateByAdmin(int adminId, int customerId, UpdateByAdminCustomerDto dto)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();

        var customer = customerRepository.Find(customerId)
                       ?? throw new CustomerNotFoundException();

        if (customer.NationalCode != dto.NationalCode)
        {
            var isDuplicateNationalCode = customerRepository.IsDuplicateNationalCode(dto.NationalCode);
            if (isDuplicateNationalCode)
                throw new NationalCodeDuplicatedException();
        }

        if (customer.PhoneNumber != dto.PhoneNumber)
        {
            var isDuplicatePhoneNumber = customerRepository.IsDuplicatePhoneNumber(dto.PhoneNumber);
            if (isDuplicatePhoneNumber)
                throw new PhoneNumberDuplicatedException();
        }

        customer.FirstName = dto.FirstName;
        customer.LastName = dto.LastName;
        customer.PhoneNumber = dto.PhoneNumber;
        customer.NationalCode = dto.NationalCode;
        customer.Email = dto.Email;
        customer.Documents = dto.Documents;
        customer.IsVerified = dto.IsVerified;
        customer.JobType = dto.JobType;
        customer.MonthlyIncome = dto.MonthlyIncome;
        customer.Assets = dto.Assets;
        customerRepository.Update(customer);
        unitOfWork.Save();
    }
}