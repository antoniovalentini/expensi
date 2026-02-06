using Expensi.Domain;
using Expensi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Infrastructure.Persistence.Repositories;

public class RemittersRepository(ExpensiDbContext context) : IRemittersRepository
{
    public async Task<IEnumerable<Remitter>> GetAllAsync(Guid userId)
    {
        return await context.Remitters
            .Where(e => e.CreatedByUserId == userId)
            .ToListAsync();
    }

    public async Task<Remitter?> GetByIdAsync(Guid id, Guid userId)
    {
        return await context.Remitters
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId);
    }

    public async Task<Remitter> CreateAsync(Remitter remitter)
    {
        context.Remitters.Add(remitter);
        await context.SaveChangesAsync();
        return remitter;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var remitter = await context.Remitters
            .FirstOrDefaultAsync(e => e.Id == id && e.CreatedByUserId == userId);

        if (remitter == null)
            return false;

        context.Remitters.Remove(remitter);
        await context.SaveChangesAsync();
        return true;
    }
}
