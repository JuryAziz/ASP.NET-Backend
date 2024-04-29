
namespace Store.Models;


public class ProductCategory
{


    public required Guid CategoryId { get; set; }
    public required Guid ProductId { get; set; }

    public Product? ProductEntity { get; set; }
    public Category? CategoryEntity { get; set; }
}