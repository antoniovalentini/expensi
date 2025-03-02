namespace Expensi.Core.Models;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid UserId { get; set; }
    public List<Expense> Expenses { get; set; } = new();
}
