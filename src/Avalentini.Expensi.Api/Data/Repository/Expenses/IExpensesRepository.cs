using System.Collections.Generic;
using System.Threading.Tasks;
using Avalentini.Expensi.Core.Data.ApiContracts;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public interface IExpensesRepository
    {
        Task<IList<Expense>> GetAll(int userId);
        Task<Expense> GetSingle(int userId, string expenseId);
        Task<Expense> Add(int userId, Expense expense);
        void Edit(string id, Expense expense);
        void Remove(string id);
        bool Exists(Expense expense);
    }
}
