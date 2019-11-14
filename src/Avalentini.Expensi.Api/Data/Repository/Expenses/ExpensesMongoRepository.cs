using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalentini.Expensi.Api.Contracts.Models;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public class ExpensesMongoRepository
    {
        private readonly List<Expense> _expenses;

        public ExpensesMongoRepository()
        {
            _expenses = new List<Expense>
            {
                new Expense
                {
                    Id = "1",
                    Amount = 48M,
                    What = "Cintura RDX",
                    When = DateTime.Now,
                    Where = "Amazon",
                },
                new Expense
                {
                    Id = "2",
                    Amount = 44M,
                    What = "Lampada",
                    When = DateTime.Now.AddDays(-10),
                    Where = "OBI",
                },
                new Expense
                {
                    Id = "2",
                    Amount = 33M,
                    What = "Dominio + Hosting",
                    When = DateTime.Now.AddDays(-25),
                    Where = "Aruba",
                },
            };
        }

        public async Task<IList<Expense>> GetAll()
        {
            return await Task.FromResult(_expenses).ConfigureAwait(false);
        }
    }
}
