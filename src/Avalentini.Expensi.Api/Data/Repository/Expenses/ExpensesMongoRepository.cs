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
                }
            };
        }

        public async Task<IList<Expense>> GetAll()
        {
            return await Task.FromResult(_expenses).ConfigureAwait(false);
        }
    }
}
