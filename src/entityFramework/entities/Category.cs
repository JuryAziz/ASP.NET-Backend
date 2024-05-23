using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("Category")]
public class Category
{
    public Guid CategoryId { get; set; }

    public required string Name { get; set; }
    public string? Description { get; set; }

    public List<Product> Products { get; set; } = [];
}
