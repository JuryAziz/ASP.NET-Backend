namespace Store.Dtos;

public class ProductDto()
{
    public required Guid ProductId { get; set; }
    public required string Name { get; set; }
    public required double Price { get; set; }
    public required int Stock { get; set; }
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public string[] Images { get; set; } = [];
    public CategoryDto? Category { get; set; }
}