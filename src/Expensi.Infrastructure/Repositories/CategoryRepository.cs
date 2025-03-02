using Expensi.Core.Models;
using Expensi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Infrastructure.Repositories;

public class CategoryRepository(ExpensiDbContext context)
{
    public async Task<IEnumerable<Category>> GetAllAsync(Guid userId)
    {
        return await context.Categories
            .Where(c => c.UserId == userId)
            .ToListAsync();
    }

    public async Task<Category?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
    }

    public async Task<Category> CreateAsync(Category category)
    {
        context.Categories.Add(category);
        await context.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> UpdateAsync(Category category)
    {
        var existingCategory = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == category.Id && c.UserId == category.UserId);

        if (existingCategory == null)
            return null;

        context.Entry(existingCategory).CurrentValues.SetValues(category);
        await context.SaveChangesAsync();
        return existingCategory;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

        if (category == null)
            return false;

        context.Categories.Remove(category);
        await context.SaveChangesAsync();
        return true;
    }
}
