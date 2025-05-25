using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Expensi.UIClient.Dtos;

namespace Expensi.UIClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IExpensiClient _client;

    [ObservableProperty]
    private ExpenseDto? _newExpense;

    [ObservableProperty]
    private string _totals = string.Empty;

    public ObservableCollection<ExpenseDto> Expenses { get; } = [];

    public MainWindowViewModel(IExpensiClient client)
    {
        _client = client;
        _ = Init();
    }

    private async Task Init()
    {
        // GET /expenses
        var expenses = await _client.GetExpensesAsync();

        Expenses.Clear();
        foreach (var expenseDto in expenses)
        {
            Expenses.Add(expenseDto);
        }

        if (Expenses.Count == 0) return;

        var totals = new Dictionary<string, decimal>();
        foreach (var expense in Expenses)
        {
            if (totals.ContainsKey(expense.Remitter))
            {
                totals[expense.Remitter] += expense.Amount;
            }
            else
            {
                totals[expense.Remitter] = expense.Amount;
            }
        }

        Totals = string.Join(" - ", totals.Select(kvp => $"{kvp.Key}: € {kvp.Value}"));
    }
}
