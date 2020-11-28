using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Avalentini.Expensi.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Avalentini.Expensi.WebApp.Pages.Expenses
{
    public class Create : PageModel
    {
        private readonly ExpenseService _expenseService;

        [BindProperty] public ExpenseViewModel Expense { get; set; }

        [TempData] public string Message { get; set; }

        public Create(ExpenseService expenseService)
        {
            _expenseService = expenseService;
        }

        public void OnGet()
        {
            Expense = new ExpenseViewModel {When = DateTime.Now};
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Expense.When = DateTime.Now;
            // TODO: replace with real user id
            await _expenseService.Add(1, Expense);

            Message = "Expense created successfully!";

            return RedirectToPage("../Index");
        }
    }

    public class ExpenseViewModel
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "The field Amount must be greater than 0.01")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required] public DateTime When { get; set; }
        [Required] public string Where { get; set; }
        [Required] public string What { get; set; }
    }
}
