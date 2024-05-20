using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.Application.Services;
public class CategoriesService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<Category>> GetAllCategories()
    {
       return await _appDbContext.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategoryById(Guid categoryId)
    {
        return await Task.FromResult((await GetAllCategories()).FirstOrDefault(c => c.CategoryId == categoryId));
    }

    public async Task<Category> CreateCategory(CategoryModel newCategory)
    {
        Category category = new Category
        {
            Name = newCategory.Name,
            Description = newCategory.Description,
        };

        await _appDbContext.Categories.AddAsync(category);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(category);
    }

    public async Task<Category?> UpdateCategory(Guid categoryId, CategoryModel updateCategory)
    {
        Category? category = await GetCategoryById(categoryId);
        if (category is null) return null;

        category.Name = updateCategory.Name;
        category.Description = updateCategory.Description;

        await _appDbContext.SaveChangesAsync();
        return await Task.FromResult(category);
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        var CategoryToDelete = await GetCategoryById(categoryId);
        if (CategoryToDelete is null) return await Task.FromResult(false);

        _appDbContext.Categories.Remove(CategoryToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}