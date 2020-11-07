using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Contracts.Models;
using Avalentini.Expensi.Core.Data.Entities;
using MongoDB.Driver;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public class ExpensesMongoRepository
    {
        private readonly IMongoCollection<ExpensesPerUser> _collection;
        private readonly IMapper _mapper;
        private ExpensesPerUser _userExpenses;
        
        public ExpensesMongoRepository(IMongoCollection<ExpensesPerUser> collection, IMapper mapper)
        {
            _mapper = mapper;
            _collection = collection;
        }

        public async Task<IList<Expense>> GetAll(int userId)
        {
            var userExpenses = await FetchUserExpenses(userId).ConfigureAwait(false);
            var result = userExpenses.Expenses
                .Select(exp => _mapper.Map<Expense>(exp)).
                ToList();
            return await Task.FromResult(result).ConfigureAwait(false);
        }

        public async Task<Expense> Get(int userId, string expenseId)
        {
            var entity = (await FetchUserExpenses(userId).ConfigureAwait(false))
                .Expenses
                .FirstOrDefault(e => e.ExpenseId == expenseId);
            return await Task.FromResult(entity == null ? null : _mapper.Map<Expense>(entity)).ConfigureAwait(false);
        }

        public void Add(Expense expense)
        {
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);
            entity.CreationDate = DateTime.Now;
            entity.ExpenseId = Guid.NewGuid().ToString();
            _userExpenses.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _userExpenses.UserId, _userExpenses);
        }

        public void Edit(string id, Expense expense)
        {
            if (expense != null && expense.Id != id)
                return;

            var oldEntity = _userExpenses.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            if (oldEntity == null)
                return;
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);

            // TODO: preserve creation date (find a smarter way to do it)
            entity.CreationDate = oldEntity.CreationDate;

            _userExpenses.Expenses.Remove(oldEntity);
            _userExpenses.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _userExpenses.UserId, _userExpenses);
        }

        public void Remove(string id)
        {
            var entity = _userExpenses.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            // it's already checked inside the controller
            // two checks means 2 calls... should we remove 1?
            if (entity == null)
                return;

            _userExpenses.Expenses.Remove(entity);
            _collection.ReplaceOne(eu => eu.UserId == _userExpenses.UserId, _userExpenses);
        }

        public bool Exists(Expense expense)
        {
            return _userExpenses.Expenses.Any(e => e.ExpenseId == expense.Id);
        }

        private async Task<ExpensesPerUser> FetchUserExpenses(int userId)
        {
            if (_userExpenses is {}) return _userExpenses;
            
            var element = _collection.Find(e => e.UserId == userId);
            _userExpenses =  await element.CountDocumentsAsync().ConfigureAwait(false) > 0
                ? element.ToList().First() : new ExpensesPerUser();

            return _userExpenses;
        }
    }
}
