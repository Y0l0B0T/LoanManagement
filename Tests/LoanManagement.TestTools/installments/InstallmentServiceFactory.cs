using LoanManagement.Persistence.Ef;
using LoanManagement.Persistence.Ef.installments;
using LoanManagement.Persistence.Ef.Loans;
using LoanManagement.Persistence.Ef.UnitOfWorks;
using LoanManagement.Services.installments;
using LoanManagement.Services.installments.Contracts.Interfaces;

namespace LoanManagement.TestTools.installments;

public static class InstallmentServiceFactory
{
    public static InstallmentAppService Generate(EfDataContext context)
    {
        var loanRepository = new EFLoanRepository(context);
        var installmentRepository = new EFInstallmentRepository(context);
        var unitOfWork = new EfUnitOfWork(context);
        return new InstallmentAppService(loanRepository, installmentRepository, unitOfWork);
    }
}