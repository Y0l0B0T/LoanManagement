namespace LoanManagement.Services.installments.Contracts.Interfaces;

public interface InstallmentRepository
{
    Installment? Find(int installmentId);
    void Update(Installment installment);
}