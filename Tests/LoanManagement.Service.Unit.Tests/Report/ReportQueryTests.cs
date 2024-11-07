using FluentAssertions;
using LoanManagement.Entities.Customers;
using LoanManagement.Entities.installments;
using LoanManagement.Entities.Loans;
using LoanManagement.Persistence.Ef.Report;
using LoanManagement.Services.Loans.Contracts.DTOs;
using LoanManagement.Services.Report.Contracts.DTOs;
using LoanManagement.Services.Report.Contracts.Interface;
using LoanManagement.TestTools.Customers;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;
using LoanManagement.TestTools.installments;
using LoanManagement.TestTools.Loans;
using LoanManagement.TestTools.LoansDefinition;
using Xunit;

namespace LoanManagement.Service.Unit.Tests.Report;

public class ReportQueryTests : BusinessIntegrationTest
{
    private readonly ReportQuery _sut;

    public ReportQueryTests()
    {
        _sut = new EFReportQuery(ReadContext);
    }

    [Fact]
    public void ReportActiveLoan_report_active_loan_properly()
    {
        var customer = new CustomerBuilder()
            .WithJobType(JobType.Employee)
            .WithAssets(20000000)
            .WithMonthlyIncome(7000000)
            .WithDocuments("madarek")
            .IsVerified(true)
            .Build();
        Save(customer);
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(6)
            .WithLoanAmount(10000000).Build();
        Save(loanDefinition);
        
        
        var loan = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan);
        var installment = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,01,01))
            .Build();
        Save(installment);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,02,01))
            .Build();
        Save(installment2);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan2);
        var installment3 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithStatus(InstallmentStatus.PaidOnTime)
            .WithDueTime(new DateOnly(2020,03,01))
            .WithPaymentTime(new DateOnly(2020,03,01))
            .Build();
        Save(installment3);
        var installment4 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .WithDueTime(new DateOnly(2020,04,01))
            .WithPaymentTime(new DateOnly(2020,04,02))
            .Build();
        Save(installment4);
        
        var loan3 = new LoanBuilder()
            .WithCustomerId(customer.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .WithValidationScore(65)
            .WithLoanType("ShortTerm")
            .Build();
        Save(loan3);
        var installment5 = new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,05,01))
            .Build();
        Save(installment5);
        var installment6 = new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithStatus(InstallmentStatus.Pending)
            .WithDueTime(new DateOnly(2020,06,01))
            .Build();
        Save(installment6);

        var actual = _sut.ReportActiveLoan();
        
        actual.Should().HaveCount(3);
        actual.Should().ContainEquivalentOf(new ReportActiveLoanDto
        {
            Id = loan.Id,
            CustomerId = loan.CustomerId,
            Status = loan.Status,
            LoanType = loan.LoanType,
            PaymentAmount = 0,
            PendingInstallments = 
            [
                new GetPendingInstallmentsByLoanId
                {
                    Id = installment.Id,
                    DueTime = installment.DueTime,
                    Status = installment.Status,
                },
                new GetPendingInstallmentsByLoanId
                {
                    Id = installment2.Id,
                    DueTime = installment2.DueTime,
                    Status = installment2.Status,
                }
            ]
        });

        actual.Should().ContainEquivalentOf(new ReportActiveLoanDto
        {
            Id = loan2.Id,
            CustomerId = loan2.CustomerId,
            Status = loan2.Status,
            LoanType = loan2.LoanType,
            PaymentAmount = (((loanDefinition.InstallmentAmount) * 2) +
                             loanDefinition.MonthlyPenaltyAmount),
            PendingInstallments = []
        });
        actual.Should().ContainEquivalentOf(new ReportActiveLoanDto
        {
            Id = loan3.Id,
            CustomerId = loan3.CustomerId,
            Status = loan3.Status,
            LoanType = loan3.LoanType,
            PaymentAmount = 0,
            PendingInstallments = 
            [
                new GetPendingInstallmentsByLoanId
                {
                    Id = installment5.Id,
                    DueTime = installment5.DueTime,
                    Status = installment5.Status,
                },
                new GetPendingInstallmentsByLoanId
                {
                    Id = installment6.Id,
                    DueTime = installment6.DueTime,
                    Status = installment6.Status,
                }
            ]
        });
    }

    [Fact]
    public void ReportAllRiskyCustomers_report_all_risky_customers_properly()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);
        var customer3 = new CustomerBuilder().Build();
        Save(customer3);
        var customer4 = new CustomerBuilder().Build();
        Save(customer4);
        
        var loanDefinition = new LoanDefinitionBuilder().WithLoanAmount(3).Build();
        Save(loanDefinition);
        
        var loan1 = new LoanBuilder()
            .WithCustomerId(customer1.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .Build();
        Save(loan1);
        
        var installment1 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment1);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment2);
        var installment3 =new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment3);
        
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer2.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan2);
        
        var installment4 =new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment4);
        var installment5 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment5);
        var installment6 = new InstallmentBuilder()
                .WithLoanId(loan2.Id)
                .WithDueTime(today.AddMonths(3))
                .WithPaymentTime(today.AddMonths(4))
                .WithStatus(InstallmentStatus.PaidWithDelay)
                .Build();
        Save(installment6);
        
        var loan3 = new LoanBuilder()
            .WithCustomerId(customer3.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan3);
        
        var installment7 = new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment7);
        var installment8 = new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment8);
        var installment9 = new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment9);
        
        var loan4 = new LoanBuilder()
            .WithCustomerId(customer4.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.DelayInPaying)
            .Build();
        Save(loan4);
        
        var installment10 = new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(-2))
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment10);
        var installment11 = new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(-1))
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment11);
        var installment12 = new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment12);
        
        var loan5 = new LoanBuilder()
            .WithCustomerId(customer4.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan5);
        
        var installment13 = new InstallmentBuilder()
            .WithLoanId(loan5.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment13);
        var installment14 = new InstallmentBuilder()
            .WithLoanId(loan5.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(4))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment14);
        var installment15 = new InstallmentBuilder()
            .WithLoanId(loan5.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(4))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment15);

        var actual = _sut.ReportAllRiskyCustomers();
        
        actual.Should().HaveCount(2);

        actual.Should().ContainEquivalentOf(new ReportAllRiskyCustomersDto
        {
            CustomerId = customer2.Id,
            FirstName = customer2.FirstName,
            LastName = customer2.LastName,
            NationalCode = customer2.NationalCode,
            DelayedInstallmentCount = 3
        });
        actual.Should().ContainEquivalentOf(new ReportAllRiskyCustomersDto
        {
            CustomerId = customer4.Id,
            FirstName = customer4.FirstName,
            LastName = customer4.LastName,
            NationalCode = customer4.NationalCode,
            DelayedInstallmentCount = 4
        });
    }
    [Fact]
    public void ReportMonthlyIncome_report_monthly_balance_income_properly()
    {
        var testDate = new DateOnly(2024, 11, 01);
        var today = DateOnly.FromDateTime(DateTime.Today);
        var customer1 = new CustomerBuilder().Build();
        Save(customer1);
        var customer2 = new CustomerBuilder().Build();
        Save(customer2);

        var loanDefinition = new LoanDefinitionBuilder().WithLoanAmount(3).Build();
        Save(loanDefinition);
        
        var loan1 = new LoanBuilder()
            .WithCustomerId(customer1.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .Build();
        Save(loan1);
        
        var installment1 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(testDate)
            .WithPaymentTime(testDate)
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment1);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(testDate)
            .WithPaymentTime(testDate)
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment2);
        var installment3 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(testDate)
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment3);
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer2.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan2);
        var installment4 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(testDate.AddMonths(-1))
            .WithPaymentTime(testDate)
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment4);
        var installment5 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment5);
        var installment6 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment6);


        var actual = _sut.ReportMonthlyIncome(testDate);
        
        actual.Should().HaveCount(2);
        actual.First().TotalIncomeFromInterest.Should().Be(0.08m);
        actual.First().TotalIncomeFromPenalty.Should().Be(0.0m);
    }
    [Fact]
    public void ReportAllClosedLoan_report_all_closed_loan_with_details_properly()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        
        var customer1 = new CustomerBuilder()
            .WithFirstName("hosein")
            .WithLastName("hoseini")
            .WithNationalCode("1111111111")
            .Build();
        Save(customer1);
        var customer2 = new CustomerBuilder()
            .WithFirstName("reza")
            .WithLastName("rezaii")
            .WithNationalCode("2222222222").Build();
        Save(customer2);
        var customer3 = new CustomerBuilder()
            .WithFirstName("mohamad")
            .WithLastName("mohammadi")
            .WithNationalCode("3333333333").Build();
        Save(customer3);
        var customer4 = new CustomerBuilder()
            .WithFirstName("javad")
            .WithLastName("javadi")
            .WithNationalCode("4444444444").Build();
        Save(customer4);
        
        var loanDefinition = new LoanDefinitionBuilder()
            .WithInstallmentsCount(3).Build();
        Save(loanDefinition);
        
        var loan1 = new LoanBuilder()
            .WithCustomerId(customer1.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Paying)
            .Build();
        Save(loan1);
        var loan2 = new LoanBuilder()
            .WithCustomerId(customer2.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan2);
        var loan3 = new LoanBuilder()
            .WithCustomerId(customer3.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan3);
        var loan4 = new LoanBuilder()
            .WithCustomerId(customer3.Id)
            .WithLoanDefinitionId(loanDefinition.Id)
            .WithStatus(LoanStatus.Closed)
            .Build();
        Save(loan4);
        
        //Loan_1_Installments_______________________
        var installment1 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today)
            .WithPaymentTime(today)
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment1);
        var installment2 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment2);
        var installment3 = new InstallmentBuilder()
            .WithLoanId(loan1.Id)
            .WithDueTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.Pending)
            .Build();
        Save(installment3);
        
        //Loan_2_Installments_______________________
        var installment4 = new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment4);
        var installment5 =new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment5);
        var installment6 =new InstallmentBuilder()
            .WithLoanId(loan2.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment6);
        
        //Loan_3_Installments_______________________
        var installment7 =new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment7);
        var installment8 =new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment8);
        var installment9 =new InstallmentBuilder()
            .WithLoanId(loan3.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(3))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment9);
        
        //Loan_4_Installments_______________________
        var installment10 = new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(1))
            .WithPaymentTime(today.AddMonths(1))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment10);
        var installment11 = new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(2))
            .WithPaymentTime(today.AddMonths(2))
            .WithStatus(InstallmentStatus.PaidOnTime)
            .Build();
        Save(installment11);
        var installment12 =new InstallmentBuilder()
            .WithLoanId(loan4.Id)
            .WithDueTime(today.AddMonths(3))
            .WithPaymentTime(today.AddMonths(4))
            .WithStatus(InstallmentStatus.PaidWithDelay)
            .Build();
        Save(installment12);
        
        var actual = _sut.ReportAllClosedLoan();

        // actual.Should().HaveCount(3);
        actual.Should().ContainEquivalentOf(new ReportAllClosedLoanDto
        {
            Id = loan2.Id,
            CustomerId = loan2.CustomerId,
            LoanDefinitionId = loan2.LoanDefinitionId,
            FirstName = customer2.FirstName,
            LastName = customer2.LastName,
            NationalCode = customer2.NationalCode,
            InstallmentsCount = loanDefinition.InstallmentsCount,
            LoanAmount = loanDefinition.LoanAmount,
            LoanType = loan2.LoanType,
            TotalPenaltyAmount = Math.Round(2 * loanDefinition.MonthlyPenaltyAmount, 2)
        });
        actual.Should().ContainEquivalentOf(new ReportAllClosedLoanDto
        {
            Id = loan3.Id,
            CustomerId = loan3.CustomerId,
            LoanDefinitionId = loan3.LoanDefinitionId,
            FirstName = customer3.FirstName,
            LastName = customer3.LastName,
            NationalCode = customer3.NationalCode,
            InstallmentsCount = loanDefinition.InstallmentsCount,
            LoanAmount = loanDefinition.LoanAmount,
            LoanType = loan3.LoanType,
            TotalPenaltyAmount = Math.Round(2 * loanDefinition.MonthlyPenaltyAmount, 2)
        });
        actual.Should().ContainEquivalentOf(new ReportAllClosedLoanDto
        {
            Id = loan4.Id,
            CustomerId = loan4.CustomerId,
            LoanDefinitionId = loan4.LoanDefinitionId,
            FirstName = customer3.FirstName,
            LastName = customer3.LastName,
            NationalCode = customer3.NationalCode,
            InstallmentsCount = loanDefinition.InstallmentsCount,
            LoanAmount = loanDefinition.LoanAmount,
            LoanType = loan4.LoanType,
            TotalPenaltyAmount = Math.Round(2 * loanDefinition.MonthlyPenaltyAmount, 2)
        });
    }
}