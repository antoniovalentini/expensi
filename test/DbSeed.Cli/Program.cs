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
        var expenses = await ParseCsv(csvFilePath);
        var httpClient = new HttpClient();

        foreach (var expense in expenses)
        {
            var jsonContent = JsonSerializer.Serialize(expense);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:5038/api/expenses", content);
            Console.WriteLine($"Sent: {expense.Title} - Status: {response.StatusCode}");
        }
    }

    private static async Task<List<CreateExpenseDto>> ParseCsv(string filePath)
    {
        const string url = "http://localhost:5038/api/categories/";
        var response = await Client.GetAsync(url);
        var content = await response.Content.ReadAsStringAsync();
        var categories = JsonSerializer.Deserialize<CategoryDto[]>(content, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower});
        if (categories == null || categories.First().Id == Guid.Empty)
        {
            throw new Exception("Failed to fetch categories");
        }

        var expenses = new List<CreateExpenseDto>();

        var categoryTranslation = new Dictionary<string, string>
        {
            { "Servizi", "Services" },
            { "Utenze", "Utilities" },
            { "Casa", "Home" },
            { "Cibo e casalinghi", "Food and Household Items" },
            { "Hobby", "Hobby" },
            { "Extra", "Extra" },
            { "Salute", "Health" },
            { "Auto", "Car" },
            { "Pranzi e cene fuori", "Dining Out" },
            { "Viaggi", "Travel" }
        };

        var lines = await File.ReadAllLinesAsync(filePath);
        for (var i = 1; i < lines.Length; i++) // Skip header
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;
            var columns = lines[i].Split('\t');

            var title = columns[2].Trim();
            var day = int.Parse(columns[1].Trim());
            var category = columns[3].Trim();

            Guid? remitterId;
            decimal amount;

            switch (columns.Length)
            {
                case 5:
                    amount = TryParseDecimal(columns[4]) ?? throw new Exception("Failed to parse amount 1");
                    remitterId = Guid.Parse("649fd541-6b02-4039-8a22-a53178afb471");
                    break;
                case 6:
                    amount = TryParseDecimal(columns[5]) ?? throw new Exception("Failed to parse amount 2");
                    remitterId = Guid.Parse("857648af-6d44-4d97-9791-7f001bd1890f");
                    break;
                case 7:
                    amount = TryParseDecimal(columns[6]) ?? throw new Exception("Failed to parse amount 3");
                    remitterId = Guid.Parse("eb7cd4e1-4495-44ff-ae02-3e53ce7e4969");
                    break;
                default:
                    throw new Exception("Invalid number of columns in row: " + lines[i]);
            }

            var categoryId = categories.First(c => c.Name.Equals(categoryTranslation.GetValueOrDefault(category))).Id;

            if (amount == 0)
            {
                Console.WriteLine("Skipping invalid row: " + lines[i]);
                continue;
            } // Skip invalid categories or zero amounts

            var dateTime = new DateTime(2025, 2, day); // ISO 8601
            var expense = new CreateExpenseDto
            (
                title,
                "Expense registered from CSV import",
                amount,
                "EUR",
                DateTime.SpecifyKind(dateTime, DateTimeKind.Utc),
                categoryId,
                remitterId
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

