using Expensi.Domain.Entities;

namespace Expensi.Domain;

public interface IRemittersRepository
{
    Task<IEnumerable<Remitter>> GetAllAsync(Guid userId);
    Task<Remitter?> GetByIdAsync(Guid id, Guid userId);
    Task<Remitter> CreateAsync(Remitter expense);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}
