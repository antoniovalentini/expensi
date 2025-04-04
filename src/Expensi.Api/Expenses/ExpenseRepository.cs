﻿using Expensi.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Api.Expenses;

public class ExpenseRepository(ExpensiDbContext context)
{
    public async Task<IEnumerable<Expense>> GetAllAsync(Guid userId)
    {
        return await context.Expenses
            .Where(e => e.CreatedByUserId == userId)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId);
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
            .FirstOrDefaultAsync(e => e.Id == expense.Id && e.CreatedByUserId == expense.CreatedByUserId);

        if (existingExpense == null)
            return null;

        context.Entry(existingExpense).CurrentValues.SetValues(expense);
        await context.SaveChangesAsync();
        return existingExpense;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var expense = await context.Expenses
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId);

        if (expense == null)
            return false;

        context.Expenses.Remove(expense);
        await context.SaveChangesAsync();
        return true;
    }
}
