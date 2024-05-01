using Store.Models;

namespace Store.Application.Services;

public class CategoriesService
{
    public readonly static List<CategoryModel> _categories = [
         new CategoryModel
            {
                _categoryId = Guid.Parse("74ddc01e-5bef-410a-a0cd-bbbbdfa6e90c"),
                //CategoryId = Guid.Parse("74ddc01e-5bef-410a-a0cd-bbbbdfa6e90c"),
                Name = "a1",
                 Description = " a1 a1 "
            },
            new CategoryModel
            {
                _categoryId = Guid.Parse("917371ce-4ebb-4123-b8da-3e3db9290bbb"),
                Name = "a2",
                 Description = " a2 a2 "
            },
            new CategoryModel
            {
                _categoryId = Guid.Parse("e5806bcb-6e7f-4ac2-a0d5-251dfe7265ea"),
                Name = "a3",
                 Description = " a3 a3 "
            },

            new CategoryModel
            {
                _categoryId = Guid.Parse("733a602e-4496-4cb3-98b9-dfef76220e7d"),
                Name = "a4",
                 Description = " a4 a4 "
            },
        new CategoryModel
            {
                _categoryId = Guid.Parse("7238a64d-6b6f-4221-b79b-45395cf4def1"),
                Name = "a5",
                 Description = " a5 a5 "
            },
    ];

    public async Task<IEnumerable<CategoryModel>> GetAllCategoriesService()
    {
        await Task.Delay(1500); // simulate delay
        return _categories.AsEnumerable();
    }

    public async Task<CategoryModel?> GetCategoryById(Guid categoryId)
    {
        await Task.Delay(1500); // simulate delay

        return _categories.Find(category => category.CategoryId == categoryId);
    }

    public async Task<CategoryModel> CreateCategoryService(CategoryModel newCategory)
    {
        await Task.Delay(1500); // simulate delay

        CategoryModel categoryTemplet = new()
        {
            _categoryId = Guid.NewGuid(),
            Name = newCategory.Name,
            Description = newCategory.Description
        };
        _categories.Add(categoryTemplet);
        return categoryTemplet;
    }

    public async Task<CategoryModel?> UpdateCategoryService(Guid categoryId, CategoryModel newCategory)
    {
        await Task.Delay(1500); // simulate delay
        var existingCategory = _categories.FirstOrDefault(u => u.CategoryId == categoryId);

        if (existingCategory is not null)
        {
            existingCategory.Name = newCategory.Name;
            existingCategory.Description = newCategory.Description;
        }

        return existingCategory;
    }

    public async Task<bool> DeleteCategoryService(Guid categoryId)
    {
        await Task.Delay(1500); // simulate delay
        var removingCategory = _categories.FirstOrDefault(u => u.CategoryId == categoryId);

        if (removingCategory != null)
        {
            _categories.Remove(removingCategory);
            return true;
        }
        return false;
    }
}