using Expensi.UIClient.Dtos;

namespace Expensi.UIClient.ViewModels;

public class DesignMainWindowViewModel() : MainWindowViewModel(new FakeExpensiClient())
{
    private static readonly List<string> FakeCategories =
    [
        "Category 1",
        "Category 2",
        "Category 3",
    ];

    private static readonly List<ExpenseDto> FakeExpenses =
    [
        new(
            Guid.NewGuid(),
            "Expense 1",
            100,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
            FakeCategories[0],
            "Member 1",
            Guid.AllBitsSet
        ),
        new(
            Guid.NewGuid(),
            "Expense 2",
            200,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
            FakeCategories[1],
            "Member 2",
            Guid.AllBitsSet
        ),
        new(
            Guid.NewGuid(),
            "Expense 3",
            300,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-20),
            FakeCategories[2],
            "Member 1",
            Guid.AllBitsSet
        )
    ];

    private class FakeExpensiClient : IExpensiClient
    {
        public Task<IEnumerable<ExpenseDto>> GetExpensesAsync()
            => Task.FromResult<IEnumerable<ExpenseDto>>(FakeExpenses);

        public Task<IEnumerable<ExpenseDto>> GetExpensesByMonthAsync(int year, int month)
            => Task.FromResult<IEnumerable<ExpenseDto>>(FakeExpenses);
    }
}
