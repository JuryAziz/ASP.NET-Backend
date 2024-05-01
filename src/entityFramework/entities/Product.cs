using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace Store.EntityFramework.Entities;

public class Product
{
    public Guid ProductId { get; set; }

    public required string Name { get; set; }

    public required Decimal Price { get; set; }

    public required int Stock { get; set; }

    public string? Description { get; set; }

    public virtual IEnumerable<Category>? CategoryEntityList { get; set; }
}
