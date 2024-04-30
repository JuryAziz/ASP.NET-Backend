using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Store.Models;

public class ProductModel
{

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public Guid? _productId = null;


    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    private IEnumerable<CategoryModel>? _categoryEntityList { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public Guid ProductId
    {
        get => _productId ?? default;
    }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at least 20 characters long.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(1.0, 10000.0, ErrorMessage = "Price must be between 1.0 and 10000.0.")]
    public required Decimal Price { get; set; }

    [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
    public required int Stock { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string? Description { get; set; }



    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public virtual IEnumerable<CategoryModel>? CategoryEntityList
    {
        get
        {
            return _categoryEntityList;
        }
    }



    /*
    :/ 
    public static ProductModel FromEntity(Product product)
    {
        return new ProductModel
        {
            _productId = Guid.NewGuid(),
            Name = "",
            Price = 555.5m,
            Stock = 20,
            Description = "",
            _categoryEntityList = []

        };
    }*/
}