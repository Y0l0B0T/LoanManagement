using LoanManagement.Services.Report.Contracts.Interface;

namespace LoanManagement.Persistence.Ef.Report;

public class EFReportQuery(EfDataContext context) : ReportQuery
{
    public HashSet<ReportActiveLoanDto> ReportActiveLoan()
    {
        var query =
            context.Set<Loan>()
                .Where(loan => loan.Status == LoanStatus.Paying || loan.Status == LoanStatus.DelayInPaying)
                .Select(loan => new
                {
                    LoanId = loan.Id,
                    loan.CustomerId,
                    loan.Status,
                    loan.LoanType,
                    Installments = loan.Installments
                        .Select(installment => new
                        {
                            InstallmentId = installment.Id,
                            loan.LoanDefinition.InstallmentAmount,
                            PenaltyAmount = installment.PaymentTime > installment.DueTime
                                ? loan.LoanDefinition.MonthlyPenaltyAmount
                                : 0,
                            installment.DueTime,
                            InstallmentStatus = installment.Status,
                            installment.PaymentTime
                        })
                })
                .AsEnumerable()
                .Select(loan => new ReportActiveLoanDto
                {
                    Id = loan.LoanId,
                    CustomerId = loan.CustomerId,
                    Status = loan.Status,
                    LoanType = loan.LoanType,
                    PaymentAmount = loan.Installments
                        .Where(i => i.PaymentTime.HasValue || i.InstallmentStatus != InstallmentStatus.Pending)
                        .Sum(i => i.InstallmentAmount + i.PenaltyAmount),
                    PendingInstallments = loan.Installments
                        .Where(i => i.InstallmentStatus == InstallmentStatus.Pending || !i.PaymentTime.HasValue)
                        .Select(i => new GetPendingInstallmentsByLoanIdDto
                        {
                            Id = i.InstallmentId,
                            DueTime = i.DueTime,
                            Status = i.InstallmentStatus
                        })
                        .ToHashSet()
                })
                .ToHashSet();
        return query;
    }

    public HashSet<ReportAllRiskyCustomersDto> ReportAllRiskyCustomers()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var query =
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
        return query;
    }

    public ReportMonthlyIncomeDto ReportMonthlyIncome(DateOnly date)
    {
        var incomeData = context.Set<Loan>()
            .Join(context.Set<Installment>(),
                loan => loan.Id,
                installment => installment.LoanId,
                (loan, installment) => new { loan, installment })
            .Where(li => li.installment.PaymentTime.HasValue
                         && li.installment.PaymentTime.Value.Month == date.Month)
            .Select(li => new
            {
                MonthlyInterest = li.loan.LoanDefinition.MonthlyInterestAmount,
                MonthlyPenalty = li.installment.PaymentTime > li.installment.DueTime
                    ? li.loan.LoanDefinition.MonthlyPenaltyAmount
                    : 0
            });
        var totalIncome = new ReportMonthlyIncomeDto
        {
            TotalIncomeFromInterest = incomeData.Sum(x => x.MonthlyInterest),
            TotalIncomeFromPenalty = incomeData.Sum(x => x.MonthlyPenalty)
        };
        return totalIncome;
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
                    l.Installments.Count(i => i.PaymentTime > i.DueTime &&
                                              i.Status == InstallmentStatus.PaidWithDelay)
                    * l.LoanDefinition.MonthlyPenaltyAmount, 2)
            }).ToHashSet();
    }

    // public ReportMonthlyIncomeDto ReportPredictMonthlyIncome(ReportPredictDto dto)
    // {
    //     var currentDate = DateOnly.FromDateTime(DateTime.Today);
    //     
    //     var incomeData = context.Set<Loan>()
    //         .Join(context.Set<Installment>(),
    //             loan => loan.Id,
    //             installment => installment.LoanId,
    //             (loan, installment) => new { loan, installment })
    //         .Where(li => li.installment.PaymentTime.HasValue
    //                      && li.installment.PaymentTime.Value >= currentDate.AddMonths(-dto.MonthsToRetrieve)
    //                      && li.installment.PaymentTime.Value <= currentDate)
    //         .Select(li => new
    //         {
    //             MonthlyInterest = li.loan.LoanDefinition.MonthlyInterestAmount,
    //             MonthlyPenalty = li.installment.PaymentTime > li.installment.DueTime
    //                 ? li.loan.LoanDefinition.MonthlyPenaltyAmount
    //                 : 0
    //         })
    //         .ToList();
    //     
    //     var averageInterestIncome = incomeData.Average(x => x.MonthlyInterest);
    //     var averagePenaltyIncome = incomeData.Average(x => x.MonthlyPenalty);
    //     
    //     var predictedIncome = new ReportMonthlyIncomeDto
    //     {
    //         TotalIncomeFromInterest = averageInterestIncome * dto.MonthsToPredict,
    //         TotalIncomeFromPenalty = averagePenaltyIncome * dto.MonthsToPredict
    //     };
    //     return predictedIncome;
    // }
}