using Expensi.Api.Remitters.Dtos;
using Expensi.Domain;
using Expensi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Expensi.Api.Remitters;

[ApiController]
[Route("api/[controller]")]
public class RemittersController(IRemittersRepository repository) : ControllerBase
{
    // GET: api/remitters
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = HttpContext.GetUserId();
        var models = await repository.GetAllAsync(userId);
        var dtos = models.Select(model => model.ToDto());
        return Ok(dtos);
    }

    // GET: api/remitters/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(RemitterDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.GetUserId();
        var model = await repository.GetByIdAsync(id, userId);

        if (model == null)
            return NotFound();

        return Ok(model.ToDto());
    }

    // POST: api/remitters
    [HttpPost]
    [ProducesResponseType(typeof(RemitterDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateRemitterDto dto)
    {
        var userId = HttpContext.GetUserId();
        var expense = new Remitter
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            CreatedByUserId = userId,
        };

        var createdRemitter = await repository.CreateAsync(expense);
        return CreatedAtAction(nameof(GetById), new { id = createdRemitter.Id }, createdRemitter.ToDto());
    }

    // DELETE: api/remitters/{id}
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

public static class DtoExtensions
{
    public static RemitterDto ToDto(this Remitter model) => new(model.Id, model.Name);
}
