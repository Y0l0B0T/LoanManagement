using LoanManagement.Persistence.Ef;
using Xunit;

namespace LoanManagement.TestTools.Infrastructure.DataBaseConfig.Integration.
    Fixtures;

[Collection(nameof(ConfigurationFixture))]
public class EFDataContextDatabaseFixture : DatabaseFixture
{
    public static EfDataContext CreateDataContext(string tenantId)
    {
        var connectionString =
            new ConfigurationFixture().Value.ConnectionString;
        

        return new EfDataContext(
            $"server=.;database=LoanManagement_DB;Trusted_Connection=True;Encrypt=false;TrustServerCertificate=true;");
    }
}