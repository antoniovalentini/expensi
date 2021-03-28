using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalentini.Expensi.Core.Data.ApiContracts;
using Avalentini.Expensi.Core.Misc;
using Avalentini.Expensi.WebApp.Pages.Expenses;
using Newtonsoft.Json;

namespace Avalentini.Expensi.WebApp.Services
{
    public class ExpenseService
    {
        private readonly HttpClient _client;

        public ExpenseService()
        {
            _client = new HttpClient();
        }
        
        public async Task Add(int userId, ExpenseViewModel expense)
        {
            var dto = new Expense
            {
                Amount = expense.Amount,
                What = expense.What,
                When = expense.When,
                Where = expense.Where,
            };
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.Default, "application/json");
            var endpoint = Endpoints.GetEndpoint();
            var response = await _client.PostAsync(Endpoints.UrlCombine(endpoint, $"/api/expenses?userId={userId}"), content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to post expense for user: {expense.UserId}. Error: {response.StatusCode}");
            }
        }
    }
}
