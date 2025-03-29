using Expensi.Core.Dtos;
using Expensi.Core.Models;
using Expensi.Infrastructure.Repositories;
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
        var expenses = await repository.GetAllAsync(userId);
        var expenseDtos = expenses.Select(e => new ExpenseDto(
            e.Id,
            e.Title,
            e.Description,
            e.Amount,
            e.Currency,
            e.Date,
            e.CategoryId,
            e.Category?.Name ?? "Unknown",
            e.RemitterId,
            e.Remitter?.Name ?? "Family"));
        return Ok(expenseDtos);
    }

    // GET: api/expenses/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.GetUserId();
        var expense = await repository.GetByIdAsync(id, userId);

        if (expense == null)
            return NotFound();

        return Ok(new ExpenseDto(
            expense.Id,
            expense.Title,
            expense.Description,
            expense.Amount,
            expense.Currency,
            expense.Date,
            expense.CategoryId,
            expense.Category?.Name ?? "Unknown",
            expense.RemitterId,
            expense.Remitter?.Name ?? "Family"));
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
            Description = dto.Description,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            UserId = userId,
            RemitterId = dto.RemitterId
        };

        var createdExpense = await repository.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = createdExpense.Id },
            new ExpenseDto(
                createdExpense.Id,
                createdExpense.Title,
                createdExpense.Description,
                createdExpense.Amount,
                createdExpense.Currency,
                createdExpense.Date,
                createdExpense.CategoryId,
                createdExpense.Category?.Name ?? "Unknown",
                createdExpense.RemitterId,
                createdExpense.Remitter?.Name ?? "Family"));
    }

    // PUT: api/expenses/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ExpenseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateExpenseDto dto)
    {
        var userId = HttpContext.GetUserId();
        var expense = new Expense
        {
            Id = id,
            Title = dto.Title,
            Description = dto.Description,
            Amount = dto.Amount,
            Currency = dto.Currency,
            Date = dto.Date,
            CategoryId = dto.CategoryId,
            UserId = userId,
            RemitterId = dto.RemitterId
        };

        var updatedExpense = await repository.UpdateAsync(expense);

        if (updatedExpense == null)
            return NotFound();

        return Ok(new ExpenseDto(
            updatedExpense.Id,
            updatedExpense.Title,
            updatedExpense.Description,
            updatedExpense.Amount,
            updatedExpense.Currency,
            updatedExpense.Date,
            updatedExpense.CategoryId,
            updatedExpense.Category?.Name ?? "Unknown",
            updatedExpense.RemitterId,
            updatedExpense.Remitter?.Name ?? "Family"));
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
