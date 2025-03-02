using System.Globalization;
using System.Text;
using System.Text.Json;
using Expensi.Core.Dtos;

namespace DbSeed.Cli;

internal static class Program
{
    private static readonly HttpClient Client = new();

    private static async Task Main()
    {
        // await SeedCategories();
        await SeedExpenses();
    }

    private static async Task SeedCategories()
    {
        var categories = new (string Name, string Description)[]
        {
            ("Services", "Various service expenses"),
            ("Utilities", "Bills and utility expenses"),
            ("Home", "Home-related expenses"),
            ("Food and Household Items", "Groceries and household essentials"),
            ("Hobby", "Hobby-related expenses"),
            ("Extra", "Miscellaneous expenses"),
            ("Health", "Medical and healthcare expenses"),
            ("Car", "Car-related expenses"),
            ("Dining Out", "Restaurants and eating out"),
            ("Travel", "Travel and vacation expenses")
        };

        const string url = "http://localhost:5038/api/categories/";

        foreach (var category in categories)
        {
            var categoryData = new { category.Name, category.Description };
            var json = JsonSerializer.Serialize(categoryData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync(url, content);

            Console.WriteLine(response.IsSuccessStatusCode
                ? $"Successfully added category: {category.Name}"
                : $"Failed to add category: {category.Name}, Status: {response.StatusCode}");
        }
    }

    private static async Task SeedExpenses()
    {
        const string csvFilePath = "expenses.tsv";
        var expenses = ParseCsv(csvFilePath);
        var httpClient = new HttpClient();

        foreach (var expense in expenses)
        {
            var jsonContent = JsonSerializer.Serialize(expense);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync("http://localhost:5038/api/expenses", content);
            Console.WriteLine($"Sent: {expense.Title} - Status: {response.StatusCode}");
        }
    }

    private static List<CreateExpenseDto> ParseCsv(string filePath)
    {
        var expenses = new List<CreateExpenseDto>();
        var categoryMap = new Dictionary<string, string>
        {
            { "Utenze", "00fee6cc-f3a1-450b-afe5-8df08366b294" }, // Utilities
            { "Extra", "09215745-4a28-4f24-8232-c62a285adea0" }, // Extra
            { "Pranzi e cene fuori", "253d8887-89f2-404b-99b2-a3366ffecad6" }, // Dining Out
            { "Servizi", "41e8af0e-5abc-46db-88c3-fd1f0ce91f73" }, // Services
            { "Viaggi", "5f9dceba-07d2-4aca-9d4b-a49a32817c88" }, // Travel
            { "Cibo e casalinghi", "ad613e3a-9af4-4f46-9420-228a9b3f77d0" }, // Food and Household Items
            { "Salute", "b8f263fd-e12c-45d3-889e-cd9ce01c1f26" }, // Health
            { "Hobby", "cf05bc63-ca0c-40d2-b70e-a58c1b3b3000" }, // Hobby
            { "Auto", "e10fa1a8-ff7f-42c2-a357-bd9f5ecacc82" }, // Car
            { "Casa", "f66100d2-30ae-4f0e-be19-bd3de37e5dbc" } // Home
        };

        var lines = File.ReadAllLines(filePath);
        for (var i = 1; i < lines.Length; i++) // Skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var columns = lines[i].Split('\t');

            var title = columns[2].Trim();
            var day = int.Parse(columns[1].Trim());
            var category = columns[3].Trim();
            var familyAmount = TryParseDecimal(columns[4]);

            decimal? member1 = 0;
            if (columns.Length > 5)
            {
                member1 = TryParseDecimal(columns[5]);
            }

            decimal? member2 = 0;
            if (columns.Length > 6)
            {
                member2 = TryParseDecimal(columns[6]);
            }

            // Determine which amount to use
            var amount = familyAmount ?? member1 ?? member2 ?? 0;
            var categoryId = categoryMap.GetValueOrDefault(category);

            if (categoryId == null || amount == 0)
            {
                Console.WriteLine("Skipping invalid row: " + lines[i]);
                continue;
            } // Skip invalid categories or zero amounts

            var expense = new CreateExpenseDto
            (
                title,
                "Expense registered from CSV import",
                amount,
                "EUR",
                new DateTime(2025, 2, day), // ISO 8601
                Guid.Parse(categoryId)
            );

            expenses.Add(expense);
        }

        return expenses;
    }

    private static decimal? TryParseDecimal(string value)
    {
        if (value.Length < 2)
        {
            return null;
        }
        if (decimal.TryParse(value[2..], NumberStyles.Any, new CultureInfo("it-IT"), out var result))
        {
            return result;
        }
        return null;
    }
}

