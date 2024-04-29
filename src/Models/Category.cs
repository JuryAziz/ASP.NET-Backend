namespace Store.Models.Category;

public class Category
{
    public required Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
}