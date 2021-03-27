using System;
using System.Collections.Generic;
using System.Linq;
using Avalentini.Expensi.Api.Data;
using Avalentini.Expensi.Core.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Avalentini.Expensi.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            CreateDbIfNotExists(host);
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ExpensiDbContext>();
                Initialize(context);
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Startup>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        private static void Initialize(ExpensiDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Users.Any() && context.Expenses.Any())
            {
                return;   // DB has been seeded
            }

            var expenses = new List<ExpenseMongoEntity>
            {
                new ExpenseMongoEntity
                {
                    ExpenseMongoEntityId = "1",
                    Amount = 100,
                    What = "Tastiera PC",
                    When = DateTime.Now.AddDays(-50),
                    Where = "MediaWorld",
                    CreationDate = DateTime.Now,
                },
                new ExpenseMongoEntity
                {
                    ExpenseMongoEntityId = "2",
                    Amount = 50,
                    What = "Cuffie",
                    When = DateTime.Now.AddDays(-120),
                    Where = "Amazon",
                    CreationDate = DateTime.Now,
                },
            };

            //context.Expenses.AddRange(expenses);
            //context.SaveChanges();

            var user = new UserEntity
            {
                UserEntityId = 1,
                CreationDate = DateTime.Now,
                ContactEmail = "contact-email",
                Firstname = "John",
                Lastname = "Doe",
                ObjectId = Guid.NewGuid().ToString(),
                Username = "foo",
                Password = "bar",
                Expenses = expenses
            };

            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
