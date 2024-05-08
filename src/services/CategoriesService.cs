using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.Application.Services;
public class CategoriesService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<PaginationResult<CategoryModel>> GetAllCategories(string? search, int page = 1, int limit = 20)
    {
        var skip = (page - 1) * limit;
        IQueryable<Category> q = _appDbContext.Categories.Skip(skip).Take(limit);
        if (search != null)
        {
            q = q.Where(e => e.Name.Contains(search));
        }

        var totalCategoriesCount = await _appDbContext.Categories.CountAsync();
        IEnumerable<Category> list = await q.ToListAsync();
        IEnumerable<CategoryModel> categoryModelList = list.Select(e => CategoryModel.FromEntity(e));
        return new PaginationResult<CategoryModel>
        {
            Items = categoryModelList,
            TotalCount = totalCategoriesCount,
            PageNumber = page,
            PageSize = limit
        };
    }

    public async Task<CategoryModel?> GetCategoryById(Guid categoryId)
    {
        Category? category = await _appDbContext.Categories.Where(e => e.CategoryId == categoryId).SingleOrDefaultAsync();
        return category != null ? CategoryModel.FromEntity(category) : null;
    }

    public async Task<CategoryModel> CreateCategory(CategoryModel newCategory)
    {
        CategoryModel categoryModel = CategoryModel.Create(newCategory.Name, newCategory.Description);
        Category? category = await _appDbContext.Categories.Where(e => categoryModel.Name == e.Name).FirstOrDefaultAsync();
        if (category != null)
        {
            return CategoryModel.FromEntity(category);
        }

        Category _category = Category.Create(categoryModel);
        await _appDbContext.Categories.AddAsync(_category);
        await _appDbContext.SaveChangesAsync();
        return CategoryModel.FromEntity(_category);
    }

    public async Task<CategoryModel?> UpdateCategory(Guid categoryId, CategoryModel newCategory)
    {
        Category? p = await _appDbContext.Categories.Where(e => e.CategoryId == categoryId).FirstOrDefaultAsync();
        if (p == null)
        {
            return null;
        }

        p.Name = newCategory.Name;
        p.Description = newCategory.Description;
        _appDbContext.Categories.Update(p);
        await _appDbContext.SaveChangesAsync();
        return CategoryModel.FromEntity(p);
    }

    public async Task<bool> DeleteCategory(Guid categoryId)
    {
        int _deleted = await _appDbContext.Categories.Where(e => e.CategoryId == categoryId).ExecuteDeleteAsync();
        return _deleted == 1;
    }

    public async Task<IEnumerable<CategoryModel>> Seed()
    {
        foreach (var item in _categories)
        {
            await CreateCategory(item!);
        }
        return [];
    }

    public readonly static List<CategoryModel> _categories = [
            new CategoryModel
            {
                //_categoryId = Guid.Parse("74ddc01e-5bef-410a-a0cd-bbbbdfa6e90c"),
                //CategoryId = Guid.Parse("74ddc01e-5bef-410a-a0cd-bbbbdfa6e90c"),
                Name = "a1",
                 Description = " a1 a1 "
            },
            new CategoryModel
            {
                //_categoryId = Guid.Parse("917371ce-4ebb-4123-b8da-3e3db9290bbb"),
                Name = "a2",
                 Description = " a2 a2 "
            },
            new CategoryModel
            {
                //_categoryId = Guid.Parse("e5806bcb-6e7f-4ac2-a0d5-251dfe7265ea"),
                Name = "a3",
                 Description = " a3 a3 "
            },

            new CategoryModel
            {
                //_categoryId = Guid.Parse("733a602e-4496-4cb3-98b9-dfef76220e7d"),
                Name = "a4",
                 Description = " a4 a4 "
            },
        new CategoryModel
            {
                //_categoryId = Guid.Parse("7238a64d-6b6f-4221-b79b-45395cf4def1"),
                Name = "a5",
                 Description = " a5 a5 "
            },
    ];
}