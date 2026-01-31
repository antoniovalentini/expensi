using Expensi.UIClient.Dtos;

namespace Expensi.UIClient.ViewModels;

public class DesignMainWindowViewModel() : MainWindowViewModel(new FakeExpensiClient())
{
    private static readonly List<string> FakeCategories =
    [
        "Food & Dining",
        "Transportation",
        "Shopping",
        "Entertainment",
        "Health & Fitness",
        "Utilities"
    ];

    private static readonly List<string> FakeCategorySubTypes =
    [
        "Restaurant",
        "Groceries",
        "Gas Station",
        "Public Transit",
        "Online Shopping",
        "Movies",
        "Pharmacy",
        "Gym",
        "Bills"
    ];

    private static readonly List<string> FakeRemitters =
    [
        "John Doe",
        "Jane Smith",
        "Mike Johnson"
    ];

    private static readonly List<string> FakeTitles =
    [
        "Coffee Break",
        "Lunch Meeting",
        "Grocery Shopping",
        "Gas Station Fill-up",
        "Restaurant Dinner",
        "Online Purchase",
        "Pharmacy Visit",
        "Book Store",
        "Movie Tickets",
        "Taxi Ride"
    ];

    public static CreateExpenseDto CreateFakeExpense()
    {
        var random = new Random();

        var randomTitle = FakeTitles[random.Next(FakeTitles.Count)];
        var randomCategory = FakeCategories[random.Next(FakeCategories.Count)];
        var randomSubCategory = FakeCategorySubTypes[random.Next(FakeCategorySubTypes.Count)];
        var randomRemitter = FakeRemitters[random.Next(FakeRemitters.Count)];
        var randomAmount = Math.Round((decimal)(random.NextDouble() * 150 + 5), 2); // 5 to 155

        // Generate random date within current month
        var now = DateTime.UtcNow;
        var daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
        var randomDay = random.Next(1, daysInMonth + 1);
        var randomDate = new DateTime(now.Year, now.Month, randomDay);

        return new CreateExpenseDto(
            Title: randomTitle,
            Amount: randomAmount,
            Currency: "EUR",
            ReferenceDate: DateOnly.FromDateTime(randomDate),
            Category: randomCategory,
            CategorySubType: randomSubCategory,
            Remitter: randomRemitter
        );
    }

    private static readonly List<ExpenseDto> FakeExpenses =
    [
        new(
            Guid.NewGuid(),
            "Expense 1",
            100,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-1),
            FakeCategories[0],
            FakeCategorySubTypes[0],
            FakeRemitters[0],
            Guid.AllBitsSet
        ),
        new(
            Guid.NewGuid(),
            "Expense 2",
            200,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-10),
            FakeCategories[1],
            FakeCategorySubTypes[1],
            FakeRemitters[1],
            Guid.AllBitsSet
        ),
        new(
            Guid.NewGuid(),
            "Expense 3",
            300,
            "EUR",
            DateOnly.FromDateTime(DateTime.Now).AddDays(-20),
            FakeCategories[2],
            FakeCategorySubTypes[2],
            FakeRemitters[2],
            Guid.AllBitsSet
        )
    ];

    private class FakeExpensiClient : IExpensiClient
    {
        public Task<IEnumerable<ExpenseDto>> GetExpensesAsync()
            => Task.FromResult<IEnumerable<ExpenseDto>>(FakeExpenses);

        public Task<IEnumerable<ExpenseDto>> GetExpensesByMonthAsync(int year, int month)
            => Task.FromResult<IEnumerable<ExpenseDto>>(FakeExpenses);

        public Task<ExpenseDto?> CreateExpenseAsync(CreateExpenseDto expense)
        {
            var newExpense = new ExpenseDto(
                Guid.NewGuid(),
                expense.Title,
                expense.Amount,
                expense.Currency,
                expense.ReferenceDate,
                expense.Category,
                expense.CategorySubType,
                expense.Remitter,
                Guid.NewGuid()
            );
            return Task.FromResult<ExpenseDto?>(newExpense);
        }
    }
}
