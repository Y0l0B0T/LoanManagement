namespace LoanManagement.Services.installments;

public class InstallmentAppService(
    LoanRepository loanRepository,
    InstallmentRepository installmentRepository,
    UnitOfWork unitOfWork) : InstallmentService
{
    public void PayInstallment(int installmentId,PayInstallmentDto dto)
    {
        var installment = installmentRepository.Find(installmentId)
                          ?? throw new InstallmentNotFoundException();
        if (installment.PaymentTime != null || installment.Status != InstallmentStatus.Pending)
            throw new InstallmentAlreadyPaidException();
        
        installment.PaymentTime = dto.PaymentTime;
        installment.Status = dto.PaymentTime <= installment.DueTime 
            ? InstallmentStatus.PaidOnTime 
            : InstallmentStatus.PaidWithDelay;
        installmentRepository.Update(installment);
        unitOfWork.Save();
    }
}