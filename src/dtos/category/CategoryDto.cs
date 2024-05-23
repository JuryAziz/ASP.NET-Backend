
namespace Store.Dtos;

public class CategoryDto()
{
    public Guid? CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
    public List<ProductDto> Products { get; set; } = [];
}