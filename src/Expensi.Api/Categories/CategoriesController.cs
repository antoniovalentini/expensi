using Microsoft.AspNetCore.Mvc;

namespace Expensi.Api.Categories;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    // GET: api/categories
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(Categories);
    }

    private static readonly string[] Categories =
    [
        "Services",
        "Utilities",
        "Home",
        "Food and Household Items",
        "Hobby",
        "Extra",
        "Health",
        "Car",
        "Dining Out",
        "Travel",
        "Kids",
        "Transportation",
    ];
}
