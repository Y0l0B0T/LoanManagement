using LoanManagement.Entities.Loans;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Loans.Contracts.Interfaces;

namespace LoanManagement.Persistence.Ef.Loans;

public class EFLoanQuery(EfDataContext context) : LoanQuery
{
    public Loan? GetById(int loanId)
    {
        return context.Set<Loan>().FirstOrDefault(l => l.Id == loanId);
    }

    public HashSet<GetAllLoansDto> GetAll()
    {
        return context.Set<Loan>().Select(l => new GetAllLoansDto
        {
            Id = l.Id,
            CustomerId = l.CustomerId,
            LoanDefinitionId = l.LoanDefinitionId,
            CreationDate = l.CreationDate,
            LoanType = l.LoanType,
            Status = l.Status,
            ValidationScore = l.ValidationScore,
            Installments = l.Installments,
            
        }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllInReview()
    {
        return context.Set<Loan>()
            .Where(l=>l.Status == LoanStatus.UnderReview)
            .Select(l => new GetAllLoansDto
        {
            Id = l.Id,
            CustomerId = l.CustomerId,
            LoanDefinitionId = l.LoanDefinitionId,
            CreationDate = l.CreationDate,
            LoanType = l.LoanType,
            Status = l.Status,
            ValidationScore = l.ValidationScore,
            Installments = l.Installments,
            
        }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllInApproved()
    {
        return context.Set<Loan>()
            .Where(l=>l.Status == LoanStatus.Approved)
            .Select(l => new GetAllLoansDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                LoanDefinitionId = l.LoanDefinitionId,
                CreationDate = l.CreationDate,
                LoanType = l.LoanType,
                Status = l.Status,
                ValidationScore = l.ValidationScore,
                Installments = l.Installments,
            
            }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllInRejected()
    {
        return context.Set<Loan>()
            .Where(l=>l.Status == LoanStatus.Rejected)
            .Select(l => new GetAllLoansDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                LoanDefinitionId = l.LoanDefinitionId,
                CreationDate = l.CreationDate,
                LoanType = l.LoanType,
                Status = l.Status,
                ValidationScore = l.ValidationScore,
                Installments = l.Installments,
            
            }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllInPaying()
    {
        return context.Set<Loan>()
            .Where(l=>l.Status == LoanStatus.Paying)
            .Select(l => new GetAllLoansDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                LoanDefinitionId = l.LoanDefinitionId,
                CreationDate = l.CreationDate,
                LoanType = l.LoanType,
                Status = l.Status,
                ValidationScore = l.ValidationScore,
                Installments = l.Installments,
            
            }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllActiveLoans()
    {
        return context.Set<Loan>()
            .Where(l => l.Status == LoanStatus.Paying || l.Status == LoanStatus.DelayInPaying)
            .Select(l => new GetAllLoansDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                LoanDefinitionId = l.LoanDefinitionId,
                CreationDate = l.CreationDate,
                LoanType = l.LoanType,
                Status = l.Status,
                ValidationScore = l.ValidationScore,
                Installments = l.Installments,
            
            }).ToHashSet();
    }

    public HashSet<GetAllLoansDto> GetAllInClosed()
    {
        return context.Set<Loan>()
            .Where(l=>l.Status == LoanStatus.Closed)
            .Select(l => new GetAllLoansDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                LoanDefinitionId = l.LoanDefinitionId,
                CreationDate = l.CreationDate,
                LoanType = l.LoanType,
                Status = l.Status,
                ValidationScore = l.ValidationScore,
                Installments = l.Installments,
            
            }).ToHashSet();
    }
}