namespace Expensi.Domain.Entities;

public class Expense
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CategorySubType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public string Remitter { get; set; } = string.Empty;
    public DateOnly ReferenceDate { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public Guid CreatedByUserId { get; set; }
    public User? CreatedByUser { get; set; }
}
