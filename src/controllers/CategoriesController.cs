using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Services;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/categories")]
public class CategoriesController(AppDbContext appDbContext) : ControllerBase
{
    private readonly CategoriesService _categoriesService = new(appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int page = 1, [FromQuery] int limit = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        List<Category> categories = await _categoriesService.GetAllCategories();

        List<Category> sortedCategories = categories;
        switch (dir.ToLower())
        {
            case "asc":
                sortedCategories = sortedCategories.OrderBy(c => c.Name).ToList();
                break;
            case "desc":
                sortedCategories = sortedCategories.OrderByDescending(c => c.Name).ToList();
                break;
        }

        List<Category> paginatedCategories = Paginate.Function(sortedCategories, page, limit);
        return Ok(new BaseResponseList<Category>(paginatedCategories, true));
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById(string categoryId)
    {

        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid category ID Format"));

        var category = await _categoriesService.GetCategoryById(categoryIdGuid);
        if (category is null) return NotFound();

        return Ok(new BaseResponse<Category>(category, true));

    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CategoryModel newCategory)
    {

        var createdCategory = await _categoriesService.CreateCategory(newCategory);
        return CreatedAtAction(nameof(GetCategoryById), new {createdCategory?.CategoryId}, createdCategory);
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategory(string categoryId, CategoryModel updateCategory)
    {
        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest(new BaseResponse<object>(null, false, "Invalid category ID Format"));

        Category? categoryToUpdate = await _categoriesService.GetCategoryById(categoryIdGuid);
        if (categoryToUpdate is null) return BadRequest(ModelState);

        Category? updatedCategory = await _categoriesService.UpdateCategory(categoryIdGuid, updateCategory);

        return Ok(new BaseResponse<Category>(updatedCategory, true));

    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {

        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest("Invalid category ID format");

        Category? categoryToDelete = await _categoriesService.GetCategoryById(categoryIdGuid);
        var result = await _categoriesService.DeleteCategory(categoryIdGuid);
        if (!result) return NotFound();

        return Ok(new BaseResponse<Category>(categoryToDelete, true));
    }
    
}