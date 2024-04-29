namespace Store.Models;

public class Product
{
    public required Guid ProductId { get; set; }

    public required string Title { get; set; }
    public required Decimal Price { get; set; }
    public required int TotalQuantity { get; set; }
    public string Description { get; set; } = string.Empty;

    public string? Thumbnail { get; set; }

    public List<Category>? CategoryEntity { get; set; }
    public List<ProductCategory>? ProductCategoryItems { get; set; }
}