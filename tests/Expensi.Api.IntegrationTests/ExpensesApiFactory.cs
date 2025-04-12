using Expensi.Api.IntegrationTests.Expenses;
using Expensi.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Expensi.Api.IntegrationTests;

public class ExpensesApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll<IExpenseRepository>();
            services.AddSingleton<IExpenseRepository, MockExpenseRepository>();
        });
    }
}
