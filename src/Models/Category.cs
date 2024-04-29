namespace Store.Models;

public class Category
{
    public required Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    public List<Product>? ProductEntity { get; set; }

    public List<ProductCategory>? ProductCategoryItems { get; set; }
}