using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Avalentini.Expensi.Core.Data.ApiContracts;
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
            var response = await _client.PostAsync($"http://localhost:5000/api/expenses?userId={userId}", content);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception($"Failed to post expense for user: {expense.UserId}. Error: {response.StatusCode}");
            }
        }
    }
}
