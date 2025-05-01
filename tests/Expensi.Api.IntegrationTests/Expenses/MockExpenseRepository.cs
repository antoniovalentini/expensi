using Expensi.Application;
using Expensi.Domain.Entities;

namespace Expensi.Api.IntegrationTests.Expenses;

public class MockExpenseRepository : IExpenseRepository
{
    private readonly List<Expense> _expenses = [];

    public Task<IEnumerable<Expense>> GetAllAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Expense>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Expense?> GetByIdAsync(Guid id, Guid userId)
    {
        return Task.FromResult(_expenses.FirstOrDefault(e => e.Id == id && e.CreatedByUserId == userId));
    }

    public Task<Expense> CreateAsync(Expense expense)
    {
        expense.Id = Guid.NewGuid();
        _expenses.Add(expense);
        return Task.FromResult(expense);
    }

    public Task<Expense?> UpdateAsync(Expense expense)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        throw new NotImplementedException();
    }
}
