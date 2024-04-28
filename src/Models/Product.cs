namespace Store.Models.Product;

public class Product
{
    public required Guid ProductId { get; set; }
    public required Guid CategoryId { get; set; }
    public required Guid MerchantId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public required float Price { get; set; }
    public float Discount { get; set; }
    public int Stock { get; set; }
    public int Sold { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

