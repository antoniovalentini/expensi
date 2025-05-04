using Expensi.Api.IntegrationTests.Expenses;
using Expensi.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Expensi.Api.IntegrationTests;

public class ExpensesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureAppConfiguration((_, config) =>
        {
            config.AddInMemoryCollection([
                new KeyValuePair<string, string?>("PostgresConnection:Host", "localhost"),
                new KeyValuePair<string, string?>("PostgresConnection:Port", "5432"),
                new KeyValuePair<string, string?>("PostgresConnection:Database", "test_db"),
                new KeyValuePair<string, string?>("PostgresConnection:Username", "test_user"),
                new KeyValuePair<string, string?>("PostgresConnection:Password", "test_password"),
            ]);
        });

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IExpenseRepository>();
            services.AddSingleton<IExpenseRepository, MockExpenseRepository>();
        });
    }
}
