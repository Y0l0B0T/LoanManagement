using LoanManagement.Services.installments.Contracts.Interfaces;

namespace LoanManagement.Services.installments;

public class InstallmentAppService : InstallmentService
{
    // public class InstallmentService
    // {
    //     private readonly ILoanRepository _loanRepository;
    //
    //     public InstallmentService(ILoanRepository loanRepository)
    //     {
    //         _loanRepository = loanRepository;
    //     }
 
    //     public void PayInstallment(int installmentId, DateOnly paymentDate)
    //     {
    //         var installment = GetInstallmentById(installmentId);
    //         if (installment == null)
    //             throw new InstallmentNotFoundException();
    //
    //         installment.PaymentTime = paymentDate;
    //         installment.Status = paymentDate <= installment.DueTime 
    //             ? InstallmentStatus.PaidOnTime 
    //             : InstallmentStatus.PaidWithDelay;
    //
    //         UpdateInstallment(installment);
    //     }
    // }
}