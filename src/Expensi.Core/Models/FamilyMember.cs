namespace Expensi.Core.Models;

public class FamilyMember
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<Expense> Expenses { get; set; } = new();
}
