using Expensi.Core.Dtos;
using Expensi.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Expensi.Api.Expenses;

[ApiController]
[Route("api/[controller]")]
public class ExpensesController(ExpenseRepository repository) : ControllerBase
{
    // GET: api/expenses
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ExpenseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var userId = HttpContext.GetUserId();
        var models = await repository.GetAllAsync(userId);
        var dtos = models.Select(model => new ExpenseDto(
            model.Id,
            model.Title,
            model.Amount,
            model.Currency,
            model.Date,
            model.Category,
            model.Remitter,
            model.CreatedByUserId));
        return Ok(dtos);
    }

    // GET: api/expenses/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.GetUserId();
        var model = await repository.GetByIdAsync(id, userId);

        if (model == null)
            return NotFound();

        return Ok(new ExpenseDto(
            model.Id,
            model.Title,
            model.Amount,
            model.Currency,
            model.Date,
            model.Category,
            model.Remitter,
            model.CreatedByUserId));
    }

    // POST: api/expenses
    [HttpPost]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateExpenseDto dto)
    {
        var userId = HttpContext.GetUserId();
        var expense = new Expense
        {
            Id = Guid.NewGuid(),
            Title = dto.Title,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Date = dto.Date,
            Category = dto.Category,
            CreatedByUserId = userId,
            Remitter = dto.Remitter,
        };

        var createdExpense = await repository.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = createdExpense.Id },
            new ExpenseDto(
                createdExpense.Id,
                createdExpense.Title,
                createdExpense.Amount,
                createdExpense.Currency,
                createdExpense.Date,
                createdExpense.Category,
                createdExpense.Remitter,
                createdExpense.CreatedByUserId));
    }

    // PUT: api/expenses/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, ExpenseDto dto)
    {
        var userId = HttpContext.GetUserId();
        var expense = new Expense
        {
            Id = id,
            Title = dto.Title,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Date = dto.Date,
            Category = dto.Category,
            CreatedByUserId = userId,
            Remitter = dto.Remitter,
        };

        var updatedExpense = await repository.UpdateAsync(expense);

        if (updatedExpense == null)
            return NotFound();

        return Ok(new ExpenseDto(
            updatedExpense.Id,
            updatedExpense.Title,
            updatedExpense.Amount,
            updatedExpense.Currency,
            updatedExpense.Date,
            updatedExpense.Category,
            updatedExpense.Remitter,
            updatedExpense.CreatedByUserId));
    }

    // DELETE: api/expenses/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = HttpContext.GetUserId();
        var success = await repository.DeleteAsync(id, userId);

        if (!success)
            return NotFound();

        return NoContent();
    }
}
