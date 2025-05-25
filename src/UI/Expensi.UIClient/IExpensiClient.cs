using System.Net.Http.Json;
using Expensi.UIClient.Dtos;

namespace Expensi.UIClient;

public interface IExpensiClient
{
    Task<IEnumerable<ExpenseDto>> GetExpensesAsync();
}

public class ExpensiClient(HttpClient httpClient) : IExpensiClient
{
    public async Task<IEnumerable<ExpenseDto>> GetExpensesAsync()
    {
        try
        {
            var response = await httpClient.GetAsync("expenses");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseDto>>() ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Array.Empty<ExpenseDto>();
        }
    }
}
