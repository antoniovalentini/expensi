using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalentini.Expensi.Core.Data.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Avalentini.Expensi.DbSeed
{
    internal class Program
    {
        private static async Task Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables()
                .Build();

            var mongoConfig = new MongoDbConfig();
            configuration.GetSection("MongoDbConfig").Bind(mongoConfig);

            Console.WriteLine("Hello World!");

            var client = new MongoClient(mongoConfig.ConnectionString);
            var database = client.GetDatabase(mongoConfig.Database);
            var collection = database.GetCollection<ExpensesPerUser>(mongoConfig.Collection);

            var count = await TestReadAsync(collection);

            if (count > 0)
            {
                if (!ShouldProceed(collection.CollectionNamespace.FullName))
                    return;
                
                collection.DeleteMany(FilterDefinition<ExpensesPerUser>.Empty);
            }

            await FeedCollectionAsync(collection);

            Console.WriteLine("Press any key to continue...");
        }

        public static async Task FeedCollectionAsync(IMongoCollection<ExpensesPerUser> collection)
        {
            var documents = new List<ExpensesPerUser>();

            var users = 0;
            while (users <= 0)
            {
                Console.WriteLine("How many users?");
                var input = Console.ReadKey();
                Console.WriteLine();
                if (!int.TryParse(input.KeyChar.ToString(), out users))
                    Console.WriteLine("Input is not a valid number");
            }

            var expenses = 0;
            while (expenses <= 0)
            {
                Console.WriteLine("How many expenses per user?");
                var input = Console.ReadKey();
                Console.WriteLine();
                if (!int.TryParse(input.KeyChar.ToString(), out expenses))
                    Console.WriteLine("Input is not a valid number");
            }

            documents.AddRange(CreateRandomExpensesPerUsers(users, expenses));

            await collection.InsertManyAsync(documents);
        }

        private static IEnumerable<ExpensesPerUser> CreateRandomExpensesPerUsers(int userCount, int expenseCount)
        {
            var documents = new List<ExpensesPerUser>();

            for (var i = 1; i <= userCount; i++)
            {
                var user = new ExpensesPerUser {UserId = i, Expenses = new List<ExpenseMongoEntity>()};
                var rngAmount = new Random();
                var rngDateTime = new Random();
                for (var j = 0; j < expenseCount; j++)
                {
                    user.Expenses.Add(new ExpenseMongoEntity
                    {
                        Amount = rngAmount.NextDecimal(),
                        ExpenseId = Guid.NewGuid().ToString(),
                        CreationDate = DateTime.Now,
                        What = Randomizer.NextItem(),
                        Where = Randomizer.NextPlace(),
                        When = Randomizer.NextDateTime(rngDateTime),
                    });
                }

                documents.Add(user);
            }

            return documents;
        }

        public static bool ShouldProceed(string collection)
        {
            while (true)
            {
                Console.WriteLine($"We need to delete all the documents inside your current collection: {collection}.");
                Console.WriteLine("Are you sure you want to proceed? (y/n)");
                var input = Console.ReadKey();
                Console.WriteLine();

                if (input.KeyChar == 'y' || input.KeyChar == 'Y')
                    return true;
                
                if (input.KeyChar == 'n' || input.KeyChar == 'N')
                    return false;

                Console.WriteLine("Input is not a valid.");
            }
        }

        public static async Task<long> TestReadAsync(IMongoCollection<ExpensesPerUser> collection)
        {
            var count = await collection.CountDocumentsAsync(FilterDefinition<ExpensesPerUser>.Empty);
            Console.WriteLine($"Total documents: {count}\n");

            var users = await collection.Find(FilterDefinition<ExpensesPerUser>.Empty).ToListAsync();

            foreach (var user in users)
            {
                Console.WriteLine($"User {user.UserId} expenses: {user.Expenses.Count}");
                foreach (var expense in user.Expenses)
                {
                    Console.WriteLine(expense + "\n");
                }
            }

            return count;
        }
    }
}
