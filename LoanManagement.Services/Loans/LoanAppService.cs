using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.Admins.Contracts.Interfaces;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.Customers.Contracts.Interfaces;
using LoanManagement.Services.Customers.Exceptions;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;
using LoanManagement.Services.LoansDefinition.Exceptions;
using LoanManagement.Services.UnitOfWorks;

namespace LoanManagement.Services.Loans;

public class LoanAppService(
    AdminRepository adminRepository,
    CustomerRepository customerRepository,
    LoanRepository loanRepository,
    LoanDefinitionRepository loanDefinitionRepository,
    UnitOfWork unitOfWork) : LoanService
{
    public void Add(AddLoanDto dto)
    {
        var customer = customerRepository.Find(dto.CustomerId)
                       ?? throw new CustomerNotFoundException();
        
        if (customer.IsVerified == false) throw new CustomerIsNotVerifiedException();
        
        var loanDefinition = loanDefinitionRepository.Find(dto.LoanDefinitionId)
                             ?? throw new LoanDefinitionNotFoundException();
        
        if (loanRepository.IsDuplicate(customer.Id, loanDefinition.Id)) 
            throw new LoanDuplicatedException();
        
        var loan = new Loan
        {
            CustomerId = dto.CustomerId,
            LoanDefinitionId = dto.LoanDefinitionId,
            CreationDate = DateOnly.FromDateTime(DateTime.Now),
            LoanType = loanDefinition.Name
        };
        loan.ValidationScore = CalculateValidationScore(loan,customer, loanDefinition);
        loan.Status = 
            loan.ValidationScore >= 60 ? LoanStatus.UnderReview : LoanStatus.Rejected;
        
        loanRepository.Add(loan);
        unitOfWork.Save();
    }

    public void ConfirmLoan(int adminId, int loanId)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.Find(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.Status != LoanStatus.UnderReview) 
            throw new InvalidLoanStatusForConfirmationException();
            
        loan.Status = LoanStatus.Approved;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }

    public void RejectLoan(int adminId, int loanId)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.Find(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.Status != LoanStatus.UnderReview) 
            throw new InvalidLoanStatusForRejectionException();
            
        loan.Status = LoanStatus.Rejected;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }
    
    public void PayLoan(int adminId, int loanId)
    {
        
        //Bayad bad az Pay Qest ha nis Ezafe shavand
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.Find(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.Status != LoanStatus.Approved) 
            throw new InvalidLoanStatusForPayingException();
        
        // for (int i = 1; i <= loan.LoanDefinition.InstallmentsCount; i++)
        // {
        //     loan.Installments.Add(new Installment
        //     {
        //         DueTime = DateOnly.FromDateTime(DateTime.Now.AddMonths(i)),
        //     });
        // }
            
        loan.Status = LoanStatus.Paying;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }
    
    public void DelayInPayLoan(int adminId, int loanId)
    {
        //bayad baressi konad ke hadeaghal 1 qest aqab oftade
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.Find(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.Status != LoanStatus.Paying) 
            throw new InvalidLoanStatusForDelayInPayException();
            
        loan.Status = LoanStatus.DelayInPaying;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }
    
    public void ClosedLoan(int adminId, int loanId)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        var loan = loanRepository.Find(loanId)
                   ?? throw new LoanNotFoundException();
        if (loan.Status != LoanStatus.Paying) 
            throw new InvalidLoanStatusForDelayInPayException();
        if (loan.Installments != null && loan.Installments.Count != 0)
            throw new LoanHasInstallmentsException();
        
        loan.Status = LoanStatus.Closed;
        loanRepository.Update(loan);
        unitOfWork.Save();
    }


    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    private int CalculateValidationScore(Loan loan,Customer customer, LoanDefinition loanDefinition)
    {
        var score = 0;
        
        score += CalculatePreviousLoansScore(loan.CustomerId);
        
        score += CalculateIncomeScore(customer.MonthlyIncome);
        
        score += CalculateJobTypeScore(customer.JobType);
        
        score += CalculateAssetsScore(loanDefinition.LoanAmount, customer.Assets);

        return score;
    }

    private int CalculatePreviousLoansScore(int customerId)
    {
        var previousLoans = loanRepository.GetCustomerLoansById(customerId);
        var score = 0;

        foreach (var prevLoan in previousLoans.Where(l => l.Status == LoanStatus.Closed))
        {
            if (prevLoan.Installments.All(i => i.Status == InstallmentStatus.PaidOnTime))
            {
                score += 30;
            }
            else
            {
                var delayedCount = prevLoan.Installments.Count(i => i.Status == InstallmentStatus.PaidWithDelay);
                score -= (delayedCount * 5);
            }
        }
        return score;
    }

    private int CalculateIncomeScore(decimal? monthlyIncome)
    {
        if (monthlyIncome > 10000000) return 20;
        if (monthlyIncome >= 5000000 && monthlyIncome <= 10000000) return 10;
        return 0; 
    }
    private int CalculateJobTypeScore(JobType? jobType)
    {
        return jobType switch
        {
            JobType.Employee => 20,
            JobType.SelfEmployed => 10,
            JobType.UnEmployed => 0,
        };
    }

    private int CalculateAssetsScore(decimal loanAmount, decimal? assets)
    {
        var loanToAssetRatio = (loanAmount / assets) * 100;
        if (loanToAssetRatio <= 50) return 20;
        if (loanToAssetRatio <= 70) return 10;
        return 0;
    }
}