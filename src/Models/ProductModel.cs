using System.Text.Json.Serialization;

namespace Store.Models;

public class ProductModel
{
    public required Guid ProductId { get; set; }

    public required string Title { get; set; }
    public required Decimal Price { get; set; }
    public required int TotalQuantity { get; set; }
    public string? Description { get; set; }

    public string? Thumbnail { get; set; }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<CategoryModel>? CategoryEntityList { get; set; }
}