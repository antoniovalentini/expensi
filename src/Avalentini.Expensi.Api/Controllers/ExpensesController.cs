using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Avalentini.Expensi.Api.Data.Repository.Expenses;
using Avalentini.Expensi.Core.Data.ApiContracts;
using Microsoft.Extensions.Configuration;

namespace Avalentini.Expensi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpensesRepository _repo;

        public ExpensesController(IConfiguration config, IMapper mapper)
        {
            _repo = new ExpensesMongoRepository(config, mapper);
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<Expense>> Get(int userId)
        {
            return await _repo.GetAll(userId).ConfigureAwait(false);
        }
        
        // POST: api/Expenses
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] int userId, [FromBody] Expense expense)
        {
            expense = await _repo.Add(userId, expense).ConfigureAwait(false);
            return CreatedAtAction("Get", new { id = expense.Id }, expense);
        }
    }
}
