using Expensi.Domain.Entities;

namespace Expensi.Application;

public interface IExpenseRepository
{
    Task<IEnumerable<Expense>> GetAllAsync(Guid userId);
    Task<IEnumerable<Expense>> GetByDateRangeAsync(DateOnly startDate, DateOnly endDate, Guid userId);
    Task<Expense?> GetByIdAsync(Guid id, Guid userId);
    Task<Expense> CreateAsync(Expense expense);
    Task<Expense?> UpdateAsync(Expense expense);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
