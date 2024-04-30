using System.Text.Json.Serialization;

namespace Store.Models;

public class CategoryModel
{
    public required Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<ProductModel>? ProductEntity { get; set; }
}