using System.ComponentModel.DataAnnotations.Schema;

namespace Expensi.Core.Models;

// for some reason, the table name is not generated correctly (Remitter instead of Remitters)
[Table("Remitters")]
public class Remitter
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public List<Expense> Expenses { get; set; } = [];
}
