using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Store.Application.Services;

using Store.Helpers;
using Store.Models;

[ApiController]
[Route("/api/categories")]
public class CategoriesController(CategoriesService categoriesService) : ControllerBase
{
    private readonly CategoriesService _categoriesService = categoriesService;


    [HttpGet]
    public async Task<IActionResult> GetAllCategories()
    {
        IEnumerable<CategoryModel> categories = await _categoriesService.GetAllCategoriesService();
        return Ok(new BaseResponseList<CategoryModel>(categories, true));
    }




    [HttpGet("{categoryId}")]
    public async Task<IActionResult> GetCategory(string categoryId)
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

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] CategoryModel newCategory)
    {
        var createdCategory = await _categoriesService.CreateCategoryService(newCategory);



        return CreatedAtAction(nameof(GetCategory), new { categoryId = createdCategory.CategoryId }, createdCategory);
    }

    [HttpPut("{categoryId}")]
    public async Task<IActionResult> UpdateCategory(string categoryId, [FromBody] CategoryModel updateCategory)
    {


        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
        {
            return BadRequest("Invalid category ID Format");
        }

        if (updateCategory == null)
            return BadRequest(ModelState);



        var category = await _categoriesService.UpdateCategoryService(categoryIdGuid, updateCategory);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpDelete("{categoryId}")]
    public async Task<IActionResult> DeleteCategory(string categoryId)
    {
        if (!Guid.TryParse(categoryId, out Guid categoryIdGuid))
        {
            return BadRequest("Invalid category ID Format");
        }
        var result = await _categoriesService.DeleteCategoryService(categoryIdGuid);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}