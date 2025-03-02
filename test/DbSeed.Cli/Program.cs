using System.Text;
using System.Text.Json;

namespace DbSeed.Cli;

internal static class Program
{
    private static readonly HttpClient Client = new();

    static async Task Main()
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

        foreach (var category in categories)
        {
            await SendCategory(category.Name, category.Description);
        }
    }

    static async Task SendCategory(string name, string description)
    {
        const string url = "http://localhost:5038/api/categories/";
        var categoryData = new { name, description };
        var json = JsonSerializer.Serialize(categoryData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await Client.PostAsync(url, content);

        Console.WriteLine(response.IsSuccessStatusCode
            ? $"Successfully added category: {name}"
            : $"Failed to add category: {name}, Status: {response.StatusCode}");
    }
}
