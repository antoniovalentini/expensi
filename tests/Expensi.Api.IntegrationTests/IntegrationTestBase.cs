using Microsoft.Extensions.DependencyInjection;
using Expensi.Infrastructure.Persistence;

namespace Expensi.Api.IntegrationTests;

public abstract class IntegrationTestBase : IClassFixture<ExpensesApiFactory>, IDisposable
{
    protected readonly ExpensesApiFactory Factory;
    protected readonly HttpClient Client;
    protected readonly IServiceScope Scope;

    protected IntegrationTestBase(ExpensesApiFactory factory)
    {
        Factory = factory;
        Client = Factory.CreateClient();
        Scope = Factory.Services.CreateScope();
    }

    protected ExpensiDbContext GetDbContext()
    {
        return Scope.ServiceProvider.GetRequiredService<ExpensiDbContext>();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            Scope?.Dispose();
            Client?.Dispose();
        }
    }
}
