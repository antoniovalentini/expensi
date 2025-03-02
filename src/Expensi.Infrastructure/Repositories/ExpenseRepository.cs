using Expensi.Core.Models;
using Expensi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Infrastructure.Repositories;

public class ExpenseRepository(ExpensiDbContext context)
{
    public async Task<IEnumerable<Expense>> GetAllAsync(Guid userId)
    {
        return await context.Expenses
            .Include(e => e.Category)
            .Include(e => e.FamilyMember)
            .Where(e => e.UserId == userId)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.Expenses
            .Include(e => e.Category)
            .Include(e => e.FamilyMember)
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
    }

    public async Task<Expense> CreateAsync(Expense expense)
    {
        context.Expenses.Add(expense);
        await context.SaveChangesAsync();
        return expense;
    }

    public async Task<Expense?> UpdateAsync(Expense expense)
    {
        var existingExpense = await context.Expenses
            .FirstOrDefaultAsync(e => e.Id == expense.Id && e.UserId == expense.UserId);

        if (existingExpense == null)
            return null;

        context.Entry(existingExpense).CurrentValues.SetValues(expense);
        await context.SaveChangesAsync();
        return existingExpense;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var expense = await context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

        if (expense == null)
            return false;

        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
        return true;
    }
}
