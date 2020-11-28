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
        private readonly IMongoCollection<UserEntity> _collection;
        private readonly IMapper _mapper;
        private UserEntity _user;
        
        public ExpensesMongoRepository(IMongoCollection<UserEntity> collection, IMapper mapper)
        {
            _mapper = mapper;
            _collection = collection;
        }

        public async Task<IList<Expense>> GetAll(int userId)
        {
            var user = await FetchUser(userId).ConfigureAwait(false);
            var result = user.Expenses
                .Select(exp => _mapper.Map<Expense>(exp)).
                ToList();
            return await Task.FromResult(result).ConfigureAwait(false);
        }

        public async Task<Expense> Get(int userId, string expenseId)
        {
            var entity = (await FetchUser(userId).ConfigureAwait(false))
                .Expenses
                .FirstOrDefault(e => e.ExpenseId == expenseId);
            return await Task.FromResult(entity == null ? null : _mapper.Map<Expense>(entity)).ConfigureAwait(false);
        }

        public void Add(Expense expense)
        {
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);
            entity.CreationDate = DateTime.Now;
            entity.ExpenseId = Guid.NewGuid().ToString();
            _user.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _user.UserId, _user);
        }

        public void Edit(string id, Expense expense)
        {
            if (expense != null && expense.Id != id)
                return;

            var oldEntity = _user.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            if (oldEntity == null)
                return;
            var entity = _mapper.Map<ExpenseMongoEntity>(expense);

            // TODO: preserve creation date (find a smarter way to do it)
            entity.CreationDate = oldEntity.CreationDate;

            _user.Expenses.Remove(oldEntity);
            _user.Expenses.Add(entity);
            _collection.ReplaceOne(eu => eu.UserId == _user.UserId, _user);
        }

        public void Remove(string id)
        {
            var entity = _user.Expenses.FirstOrDefault(e => e.ExpenseId == id);
            // it's already checked inside the controller
            // two checks means 2 calls... should we remove 1?
            if (entity == null)
                return;

            _user.Expenses.Remove(entity);
            _collection.ReplaceOne(eu => eu.UserId == _user.UserId, _user);
        }

        public bool Exists(Expense expense)
        {
            return _user.Expenses.Any(e => e.ExpenseId == expense.Id);
        }

        private async Task<UserEntity> FetchUser(int userId)
        {
            if (_user is {}) return _user;
            
            var element = _collection.Find(e => e.UserId == userId);
            _user =  await element.CountDocumentsAsync().ConfigureAwait(false) > 0
                ? element.ToList().First() : new UserEntity();

            return _user;
        }
    }
}
