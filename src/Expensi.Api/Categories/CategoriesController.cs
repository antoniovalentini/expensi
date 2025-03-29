using Expensi.Core.Dtos;
using Expensi.Core.Models;
using Expensi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Expensi.Api.Categories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(CategoryRepository repository) : ControllerBase
{
    // GET: api/categories
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var userId = HttpContext.GetUserId();
        var categories = await repository.GetAllAsync(userId);
        var categoryDtos = categories.Select(c => new CategoryDto(c.Id, c.Name, c.Description));
        return Ok(categoryDtos);
    }

    // GET: api/categories/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var userId = HttpContext.GetUserId();
        var category = await repository.GetByIdAsync(id, userId);

        if (category == null)
            return NotFound();

        return Ok(new CategoryDto(category.Id, category.Name, category.Description));
    }

    // POST: api/categories
    [HttpPost]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        var userId = HttpContext.GetUserId();
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId
        };

        await repository.CreateAsync(category);
        return CreatedAtAction(nameof(GetById), new { id = category.Id },
            new CategoryDto(category.Id, category.Name, category.Description));
    }

    // PUT: api/categories/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, CreateCategoryDto dto)
    {
        var userId = HttpContext.GetUserId();
        var category = new Category
        {
            Id = id,
            Name = dto.Name,
            Description = dto.Description,
            UserId = userId
        };

        var updatedCategory = await repository.UpdateAsync(category);

        if (updatedCategory == null)
            return NotFound();

        return Ok(new CategoryDto(updatedCategory.Id, updatedCategory.Name, updatedCategory.Description));
    }

    // DELETE: api/categories/{id}
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
