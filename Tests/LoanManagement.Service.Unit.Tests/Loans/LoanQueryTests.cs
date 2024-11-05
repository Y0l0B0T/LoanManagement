using LoanManagement.Persistence.Ef.Loans;
using LoanManagement.Services.Loans.Contracts.Interfaces;
using LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration;

namespace LoanManagement.Service.Unit.Tests.Loans;

public class LoanQueryTests: BusinessIntegrationTest
{
    private readonly LoanQuery _sut;

    public LoanQueryTests()
    {
        _sut = new EFLoanQuery(ReadContext);
    }
}