using Expensi.Api.Users.Dtos;
using Expensi.Domain.Entities;
using Expensi.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Expensi.Api.Users;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ExpensiDbContext context) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await context.Users.ToListAsync();
        var userDtos = users.Select(u => new UserDto(
            u.Id,
            u.Username,
            u.Email));
        return Ok(userDtos);
    }

    // GET: api/users/{id}
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        return Ok(new UserDto(
            user.Id,
            user.Username,
            user.Email));
    }

    // POST: api/users
    [HttpPost]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        // Check if username or email is already taken
        if (await context.Users.AnyAsync(u => u.Username == dto.Username || u.Email == dto.Email))
        {
            return BadRequest("Username or email already exists");
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.Password // In a real app, hash the password!
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id },
            new UserDto(user.Id, user.Username, user.Email));
    }

    // PUT: api/users/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, UpdateUserDto dto)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        // Check if username/email is taken by another user
        if (await context.Users.AnyAsync(u => u.Id != id &&
                                           (u.Username == dto.Username || u.Email == dto.Email)))
        {
            return BadRequest("Username or email already exists");
        }

        user.Username = dto.Username;
        user.Email = dto.Email;

        // Only update password if provided
        if (!string.IsNullOrEmpty(dto.Password))
        {
            user.PasswordHash = dto.Password; // In a real app, hash the password!
        }

        await context.SaveChangesAsync();

        return Ok(new UserDto(user.Id, user.Username, user.Email));
    }

    // DELETE: api/users/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
            return NotFound();

        context.Users.Remove(user);
        await context.SaveChangesAsync();

        return NoContent();
    }
}
