using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;
using Store.EntityFramework.Entities;

namespace Store.Application.Services;
public class CategoriesService(AppDbContext appDbContext, IMapper mapper)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryDto>> GetAllCategories()
    {
        return (await _appDbContext.Categories
            .Include(c => c.Products)
            .ToListAsync())
            .Select(_mapper.Map<CategoryDto>);
    }

    public async Task<Category?> GetCategoryById(Guid categoryId)
    {
        Category? category = await _appDbContext.Categories
            .Include(category => category.Products)
            .FirstOrDefaultAsync(category => category.CategoryId == categoryId);
        return await Task.FromResult(category);
    }

    public async Task<CategoryDto> CreateCategory(CreateCategoryDto newCategory)
    {
        Category category = new()
        {
            Name = newCategory.Name,
            Description = newCategory.Description,
        };

        await _appDbContext.Categories.AddAsync(category);
        await _appDbContext.SaveChangesAsync();

        CategoryDto DtoCategory = _mapper.Map<CategoryDto>(category);

        return await Task.FromResult(DtoCategory);
    }

    public async Task<CategoryDto?> UpdateCategory(Guid categoryId, UpdateCategoryDto updatedCategory)
    {
        Category? categoryToUpdate = await GetCategoryById(categoryId);
        if (categoryToUpdate is null) return null;

        categoryToUpdate.Name = updatedCategory.Name;
        categoryToUpdate.Description = updatedCategory.Description;

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<CategoryDto>(categoryToUpdate));
    }

    public async Task<DeleteCategoryDto?> DeleteCategory(Guid categoryId)
    {
        Category? CategoryToDelete = await GetCategoryById(categoryId);
        if (CategoryToDelete is null) return null;

        _appDbContext.Categories.Remove(CategoryToDelete);
        await _appDbContext.SaveChangesAsync();

        DeleteCategoryDto? deletedCategory = _mapper.Map<DeleteCategoryDto>(CategoryToDelete);

        return await Task.FromResult(deletedCategory);
    }
}