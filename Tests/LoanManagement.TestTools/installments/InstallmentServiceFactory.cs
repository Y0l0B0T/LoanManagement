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