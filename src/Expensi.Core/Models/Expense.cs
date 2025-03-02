namespace Expensi.Core.Models;

public class Expense
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTime Date { get; set; }
    public Guid CategoryId { get; set; }
    public Category? Category { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
}
