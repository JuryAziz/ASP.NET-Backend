using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Dtos;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/categories")]
public class CategoriesController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly CategoriesService _categoriesService = new(appDbContext, mapper);

    [HttpGet]
    public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        IEnumerable<CategoryDto> categories = await _categoriesService.GetAllCategories();

        IEnumerable<CategoryDto> sortedCategories = categories;
        switch (dir.ToLower())
        {
            case "asc":
                sortedCategories = [.. sortedCategories.OrderBy(c => c.Name)];
                break;
            case "desc":
                sortedCategories = [.. sortedCategories.OrderByDescending(c => c.Name)];
                break;
        }

        PaginationResult<CategoryDto> paginatedCategories = new() { Items = sortedCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize), TotalCount = sortedCategories.Count(), PageNumber = pageNumber, PageSize = pageSize };
        return Ok(new BaseResponse<PaginationResult<CategoryDto>>(paginatedCategories, true));
    }

    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategoryById(string categoryId)
    {

        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid category ID Format"));

        Category? category = await _categoriesService.GetCategoryById(categoryIdGuid);
        if (category is null) return NotFound();

        return Ok(new BaseResponse<Category>(category, true));

    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryDto newCategory)
    {

        var createdCategory = await _categoriesService.CreateCategory(newCategory);
        return CreatedAtAction(nameof(GetCategoryById), new {createdCategory?.CategoryId}, createdCategory);
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategory(string categoryId, UpdateCategoryDto updateCategory)
    {
        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest(new BaseResponse<object>(null, false, "Invalid category ID Format"));

        Category? categoryToUpdate = await _categoriesService.GetCategoryById(categoryIdGuid);
        if (categoryToUpdate is null) return BadRequest(ModelState);
        CategoryDto? updatedCategory = await _categoriesService.UpdateCategory(categoryIdGuid, updateCategory);

        return Ok(new BaseResponse<CategoryDto>(updatedCategory, true));

    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {

        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid)) return BadRequest("Invalid category ID format");

        Category? categoryToDelete = await _categoriesService.GetCategoryById(categoryIdGuid);
        if(categoryToDelete is null) return NotFound();
        DeleteCategoryDto? deletedCategory = await _categoriesService.DeleteCategory(categoryIdGuid);
        if (deletedCategory is null) return NotFound();

        return Ok(new BaseResponse<DeleteCategoryDto>(deletedCategory, true));
    }
    
}