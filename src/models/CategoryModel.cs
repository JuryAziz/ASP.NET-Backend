
using System.Text.Json.Serialization;

using System.ComponentModel.DataAnnotations;
using Store.EntityFramework.Entities;


namespace Store.Models;

public class CategoryModel
{

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid? _categoryId = null;


    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    private IEnumerable<ProductModel>? _productEntityList { get; set; }



    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid CategoryId
    {
        get => _categoryId ?? default;
    }



    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public required string Name { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string? Description { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual IEnumerable<ProductModel>? ProductEntityList
    {
        get
        {
            return _productEntityList;
        }
    }



    public static CategoryModel Create(string name, string? description)
    {
        return new CategoryModel
        {
            _categoryId = null,
            Name = name,
            Description = description
        };
    }

    public static CategoryModel FromEntity(Category category)
    {
        return new CategoryModel
        {
            _categoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
        };
    }
}