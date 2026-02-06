namespace Expensi.Domain.Entities;

public class Remitter
{
    public Guid Id { get; set; }
    public Guid CreatedByUserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}
