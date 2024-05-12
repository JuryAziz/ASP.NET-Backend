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
    
    // public virtual IEnumerable<Category>? CategoryEntityList { get; set; }

    public IEnumerable<Category>? CategoryList { get; set; }
    public IEnumerable<ProductCategory>? ProductCategoryList { get; set; }


    public static Product FromModel(ProductModel productModel)
    {
        return new Product
        {
            ProductId = productModel.ProductId,
            Name = productModel.Name,
            Price = productModel.Price,
            Stock = productModel.Stock,
            Description = productModel.Description,
            // CategoryEntityList = productModel.CategoryEntityList?.Select(e => Category.FromModel(e))
        };
    }

    public static Product Create(ProductModel productModel)
    {
        return new Product
        {
            Name = productModel.Name,
            Price = productModel.Price,
            Stock = productModel.Stock,
            Description = productModel.Description
        };
    }
}
