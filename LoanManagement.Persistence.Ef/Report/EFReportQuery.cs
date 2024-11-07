using System.Security.Cryptography;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Entities.LoansDefinition;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Report.Contracts.DTOs;
using LoanManagement.Services.Report.Contracts.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoanManagement.Persistence.Ef.Report;

public class EFReportQuery(EfDataContext context) : ReportQuery
{
    public HashSet<ReportActiveLoanDto> ReportActiveLoan()
    {
        var activeLoans = 
            (from loan in context.Set<Loan>()
                join installment in context.Set<Installment>() on loan.Id equals installment.LoanId
                join loanDefinition in context.Set<LoanDefinition>() on loan.LoanDefinitionId equals loanDefinition.Id
                where loan.Status == LoanStatus.Paying || loan.Status == LoanStatus.DelayInPaying
                select new
                {
                    LoanId = loan.Id,
                    CustomerId = loan.CustomerId,
                    Status = loan.Status,
                    LoanType = loan.LoanType,
                    InstallmentId = installment.Id,
                    InstallmentAmount = loanDefinition.InstallmentAmount,
                    PenaltyAmount = installment.PaymentTime > installment.DueTime ? loanDefinition.MonthlyPenaltyAmount : 0,
                    DueTime = installment.DueTime,
                    InstallmentStatus = installment.Status,
                    PaymentTime = installment.PaymentTime
                })
            .AsEnumerable()
            .GroupBy(l => new { l.LoanId, l.CustomerId, l.Status, l.LoanType })
            .Select(g => new ReportActiveLoanDto
            {
                Id = g.Key.LoanId,
                CustomerId = g.Key.CustomerId,
                Status = g.Key.Status,
                LoanType = g.Key.LoanType,
                PaymentAmount = g
                    .Where(i => i.PaymentTime.HasValue || i.InstallmentStatus != InstallmentStatus.Pending)
                    .Sum(i => i.InstallmentAmount + i.PenaltyAmount),
                PendingInstallments = g
                    .Where(i => i.InstallmentStatus == InstallmentStatus.Pending || !i.PaymentTime.HasValue)
                    .Select(i => new GetPendingInstallmentsByLoanId
                    {
                        Id = i.InstallmentId,
                        DueTime = i.DueTime,
                        Status = i.InstallmentStatus
                    })
                    .ToHashSet()
            })
            .ToHashSet();

        return activeLoans;
    }
    public HashSet<ReportAllRiskyCustomersDto> ReportAllRiskyCustomers()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var riskyCustomers = 
            (from customer in context.Set<Customer>()
                join loan in context.Set<Loan>() on customer.Id equals loan.CustomerId
                join installment in context.Set<Installment>() on loan.Id equals installment.LoanId
                where installment.Status == InstallmentStatus.PaidWithDelay ||
                      installment.PaymentTime > installment.DueTime ||
                      installment.DueTime < today
                group installment by new { customer.Id, customer.FirstName, customer.LastName, customer.NationalCode }
                into g
                where g.Count() > 2
                select new ReportAllRiskyCustomersDto
                {
                    CustomerId = g.Key.Id,
                    FirstName = g.Key.FirstName,
                    LastName = g.Key.LastName,
                    NationalCode = g.Key.NationalCode,
                    DelayedInstallmentCount = g.Count()
                })
            .ToHashSet();
        return riskyCustomers;
    }
    
    public HashSet<ReportMonthlyIncomeDto> ReportMonthlyIncome(DateOnly date)
    {
        var query = context.Set<Loan>()
            .Join(context.Set<Installment>(),
                loan => loan.Id,
                installment => installment.LoanId,
                (loan, installment) => new { loan, installment })
            .Where(li => li.installment.PaymentTime.HasValue 
                         && li.installment.PaymentTime.Value.Month == date.Month)
            .GroupBy(li => li.loan.Id)
            .Select(g => new ReportMonthlyIncomeDto
            {
                TotalIncomeFromInterest = g.Sum(li => li.loan.LoanDefinition.MonthlyInterestAmount),
                TotalIncomeFromPenalty = g.Sum(li => li.installment.PaymentTime > li.installment.DueTime
                    ? li.loan.LoanDefinition.MonthlyPenaltyAmount
                    : 0)
            })
            .ToHashSet();

        return query;
    }
    public HashSet<ReportAllClosedLoanDto> ReportAllClosedLoan()
    {
        return context.Set<Loan>()
            .Include(l => l.Customer)
            .Where(l => l.Status == LoanStatus.Closed)
            .Select(l => new ReportAllClosedLoanDto
            {
                Id = l.Id,
                CustomerId = l.CustomerId,
                FirstName = l.Customer.FirstName,
                LastName = l.Customer.LastName,
                NationalCode = l.Customer.NationalCode,
                LoanDefinitionId = l.LoanDefinitionId,
                InstallmentsCount = l.LoanDefinition.InstallmentsCount,
                LoanType = l.LoanType,
                LoanAmount = l.LoanDefinition.LoanAmount,
                TotalPenaltyAmount = Math.Round(
                    l.Installments.Count(i => i.PaymentTime > i.DueTime && i.Status == InstallmentStatus.PaidWithDelay)
                    * l.LoanDefinition.MonthlyPenaltyAmount, 2)
            }).ToHashSet();
    }
    
    
    
    

    // public HashSet<ReportAllClosedLoanDto> ReportAllClosedLoan()
    // {
    //     return context.Set<Loan>()
    //         .Include(_ => _.Installments)
    //         .Where(l => l.Status == LoanStatus.Closed)
    //         .Select(l => new ReportAllClosedLoanDto
    //         {
    //             Id = l.Id,
    //             CustomerId = l.CustomerId,
    //             FirstName = context.Set<Customer>().Where(c => c.Id == l.CustomerId).First().FirstName,
    //             LastName = context.Set<Customer>().Where(c => c.Id == l.CustomerId).Last().LastName,
    //             NationalCode = context.Set<Customer>().Where(c => c.Id == l.CustomerId).First().NationalCode,
    //             LoanDefinitionId = l.LoanDefinitionId,
    //             InstallmentsCount = l.LoanDefinition.InstallmentsCount,
    //             LoanType = l.LoanType,
    //             LoanAmount = l.LoanDefinition.LoanAmount,
    //             TotalPenaltyAmount = l.Installments
    //                                      .Count(i => i.PaymentTime > i.DueTime &&
    //                                                  i.Status == InstallmentStatus.PaidWithDelay)
    //                                  * l.LoanDefinition.MonthlyPenaltyAmount
    //         }).ToHashSet();
    // }
    // public List<GetAllClosedDto> GetAllClosed()
    // {
    //     return context.Set<Loan>().Where(l => l.LoanStatus == LoanStatus.Closed)
    //         .Select(l => new GetAllClosedDto
    //         {
    //             Id = l.Id,
    //             LoanFormatId = l.LoanFormatId,
    //             CustomerId = l.CustomerId,
    //             ValidationScore = l.ValidationScore,
    //             LoanAmount = l.LoanFormat.Amount,
    //             InstallmentsCount = l.Installments.Count,
    //             TotalPenaltyAmount =
    //                 l.Installments.Count(i => i.PaidDate > i.ShouldPayDate) *
    //                 l.LoanFormat.MonthlyPenaltyAmount,
    //         }).ToList();
    // }
}