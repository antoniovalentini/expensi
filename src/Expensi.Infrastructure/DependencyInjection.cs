using System.ComponentModel.DataAnnotations;
using Expensi.Domain;
using Expensi.Infrastructure.Persistence;
using Expensi.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Expensi.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services
            .AddOptionsWithValidateOnStart<PostgresConnectionOptions>()
            .BindConfiguration(PostgresConnectionOptions.SectionName)
            .ValidateDataAnnotations();

        services.AddDbContext<ExpensiDbContext>((sp, dbContextOptionsBuilder) =>
        {
            var npgsqlOptions = sp.GetRequiredService<IOptions<PostgresConnectionOptions>>().Value;
            dbContextOptionsBuilder.UseNpgsql(new NpgsqlConnectionStringBuilder
            {
                Host = npgsqlOptions.Host,
                Port = npgsqlOptions.Port,
                Database = npgsqlOptions.Database,
                Username = npgsqlOptions.Username,
                Password = npgsqlOptions.Password,
            }.ConnectionString);
        });

        services.AddScoped<IExpenseRepository, ExpenseRepository>();
        services.AddScoped<IRemittersRepository, RemittersRepository>();
        return services;
    }
}

public class PostgresConnectionOptions
{
    public const string SectionName = "PostgresConnection";

    [Required] public string Host { get; init; } = null!;
    [Required] public int Port { get; init; }
    [Required] public string Database { get; init; } = null!;
    [Required] public string Username { get; init; } = null!;
    [Required] public string Password { get; init; } = null!;
}
