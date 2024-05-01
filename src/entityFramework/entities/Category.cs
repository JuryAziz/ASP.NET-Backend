using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store.EntityFramework.Entities;

public class Category
{
    public Guid CategoryId { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public IEnumerable<Product>? ProductEntityList { get; set; }
}
