using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Services;

using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/categories")]
public class CategoriesController(CategoriesService categoriesService) : ControllerBase
{
    private readonly CategoriesService _categoriesService = categoriesService;

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] string? q, [FromQuery] int page = 1)
    {
        try
        {
            if (page <= 0)
            {
                return BadRequest(
                    new BaseResponse<object>(success: false, msg: "page most be more then 0 ")
                );
            }
            PaginationResult<CategoryModel> categories = await _categoriesService.GetAllCategories(q, page);
            return Ok(new BaseResponse<PaginationResult<CategoryModel>>(categories, true));
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategory(string categoryId)
    {
        try
        {
            if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
            {
                return BadRequest(new BaseResponse<object>(false, "Invalid category ID Format"));
            }

            var category = await _categoriesService.GetCategoryById(categoryIdGuid);
            if (category == null)
            {
                return NotFound();
            }

            else
            {
                return Ok(new BaseResponse<CategoryModel>(category, true));
            }
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] CategoryModel newCategory)
    {
        try
        {
            var createdCategory = await _categoriesService.CreateCategory(newCategory);
            return CreatedAtAction(nameof(GetCategory), new { categoryId = createdCategory.CategoryId }, createdCategory);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategory(string categoryId, [FromBody] CategoryModel updateCategory)
    {
        try
        {
            if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
                return BadRequest("Invalid category ID Format");

            if (updateCategory == null)
                return BadRequest(ModelState);

            var category = await _categoriesService.UpdateCategory(categoryIdGuid, updateCategory);
            if (category == null)
                return NotFound();

            return Ok(category);
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {
        try
        {
            if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
            {
                return BadRequest("Invalid category ID Format");
            }
            var result = await _categoriesService.DeleteCategory(categoryIdGuid);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await _categoriesService.Seed();
            return Created();
        }
        catch (Exception)
        {
            return StatusCode(500);
        }

    }
}