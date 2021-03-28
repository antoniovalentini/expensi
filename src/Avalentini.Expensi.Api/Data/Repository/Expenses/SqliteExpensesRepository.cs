using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Core.Data.ApiContracts;
using Avalentini.Expensi.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Avalentini.Expensi.Api.Data.Repository.Expenses
{
    public class SqliteExpensesRepository : IExpensesRepository
    {
        private readonly ExpensiDbContext _context;
        private readonly IMapper _mapper;

        public SqliteExpensesRepository(ExpensiDbContext context, IMapper mapper) =>
            (_context, _mapper) = (context, mapper);

        public async Task<IList<Expense>> GetAll(int userId)
        {
            var user = await FetchUser(userId).ConfigureAwait(false);
            var result = user.Expenses
                .Select(exp => _mapper.Map<Expense>(exp))
                .ToList();
            return result;
        }

        public Task<Expense> GetSingle(int userId, string expenseId)
        {
            throw new NotImplementedException();
        }

        public Task<Expense> Add(int userId, Expense expense)
        {
            throw new NotImplementedException();
        }

        public void Edit(string id, Expense expense)
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public bool Exists(Expense expense)
        {
            throw new NotImplementedException();
        }

        private async Task<UserEntity> FetchUser(int userId)
        {
            
            var user = await _context.Users
                .Include(x => x.Expenses)
                .FirstAsync(e => e.UserEntityId == userId)
                .ConfigureAwait(false);

            return user;
        }
    }
}
