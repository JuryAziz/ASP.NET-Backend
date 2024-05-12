using System.ComponentModel.DataAnnotations.Schema;
using Store.Models;
namespace Store.EntityFramework.Entities;

[Table("Product")]
public class Product
{
    public Guid ProductId { get; set; }
    public required string Name { get; set; }
    public required Decimal Price { get; set; }
    public required int Stock { get; set; }
    public string? Description { get; set; }

    public List<Category>? CategoryEntityList { get; set; }
    public List<Category>? CategoryList { get; set; }
    public List<ProductCategory>? ProductCategoryList { get; set; }
}
