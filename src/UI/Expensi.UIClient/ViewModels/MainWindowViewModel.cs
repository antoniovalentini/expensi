using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Expensi.UIClient.Models;
using Microsoft.Kiota.Abstractions;

namespace Expensi.UIClient.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ExpensiClient _client;

    public ObservableCollection<CategoryDto> Categories { get; } = [];
    public ObservableCollection<ExpenseDto> Expenses { get; } = [];

    public MainWindowViewModel(IRequestAdapter adapter)
    {
        _client = new ExpensiClient(adapter);
        _ = Init();
    }

    private async Task Init()
    {
        // GET /categories
        var categories = await _client.Api.Categories.GetAsync();
        if (categories is null) return;

        Categories.Clear();
        foreach (var categoryDto in categories)
        {
            Categories.Add(categoryDto);
        }

        // GET /expenses
        var expenses = await _client.Api.Expenses.GetAsync();
        if (expenses is null) return;

        Expenses.Clear();
        foreach (var expenseDto in expenses)
        {
            Expenses.Add(expenseDto);
        }
    }
}
