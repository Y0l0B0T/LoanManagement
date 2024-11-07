using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.Admins.Contracts.Interfaces;
using LoanManagement.Services.Admins.Exceptions;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using LoanManagement.Services.LoansDefinition.Contracts.DTOs;
using LoanManagement.Services.LoansDefinition.Contracts.Interfaces;
using LoanManagement.Services.UnitOfWorks;

namespace LoanManagement.Services.LoansDefinition;

public class LoanDefinitionAppService(
    AdminRepository adminRepository,
    LoanDefinitionRepository loanDefinitionRepository,
    UnitOfWork unitOfWork) : LoanDefinitionService
{ 
    public void Add(int adminId,AddLoanDefinitionDto dto)
    {
        var admin = adminRepository.Find(adminId)
                    ?? throw new AdminNotFoundException();
        
        var loanDefinition = CalculateLoanDetails(dto.LoanAmount, dto.InstallmentsCount);
        loanDefinitionRepository.Add(loanDefinition);
        unitOfWork.Save();
    }
    //-----------------------------------------Private Method For Calculating Properties
    private LoanDefinition CalculateLoanDetails(decimal loanAmount, int installmentsCount)
    {
        var loan = new LoanDefinition
        {
            LoanAmount = loanAmount,
            InstallmentsCount = installmentsCount,
            Name = SetName(installmentsCount),
            InterestRate = CalculateInterestRate(installmentsCount),
        };

        loan.MonthlyInterestAmount = Math.Round(CalculateMonthlyInterestAmount(loanAmount, loan.InterestRate), 2);
        loan.BasePaymentAmount = Math.Round(CalculateBasePaymentAmount(loanAmount, installmentsCount), 2);
        loan.InstallmentAmount = Math.Round(CalculateInstallmentAmount(loan.BasePaymentAmount, loan.MonthlyInterestAmount), 2);
        loan.MonthlyPenaltyAmount = Math.Round(CalculateMonthlyPenaltyAmount(loan.InstallmentAmount), 2);

        return loan;
    }

    private string SetName(int installmentsCount)
    {
        return installmentsCount <= 12 ? "ShortTerm" : "LongTerm";
    }

    private decimal CalculateInterestRate(int installmentsCount)
    {
        return installmentsCount <= 12 ? 0.15m : 0.20m;
    }

    private decimal CalculateMonthlyInterestAmount(decimal loanAmount, decimal interestRate)
    {
        return (interestRate / 12) * loanAmount;
    }

    private decimal CalculateBasePaymentAmount(decimal loanAmount, int installmentsCount)
    {
        return loanAmount / installmentsCount;
    }

    private decimal CalculateInstallmentAmount(decimal basePaymentAmount, decimal monthlyInterestAmount)
    {
        return basePaymentAmount + monthlyInterestAmount;
    }

    private decimal CalculateMonthlyPenaltyAmount(decimal installmentAmount)
    {
        return 0.02m * installmentAmount;
    }
}