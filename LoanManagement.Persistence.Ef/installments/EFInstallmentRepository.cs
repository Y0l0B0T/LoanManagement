namespace LoanManagement.Persistence.Ef.installments;

public class EFInstallmentRepository(EfDataContext context) : InstallmentRepository
{
    public Installment? Find(int installmentId)
    {
        return context.Set<Installment>().FirstOrDefault(i=>i.Id == installmentId);
    }

    public void Update(Installment installment)
    {
        context.Set<Installment>().Update(installment);
    }
}