using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("ShoppingList")]
public class ShoppingList
{
    public Guid ShoppingListId { get; set; }
    public required Guid UserId { get; set; }
    public required string Name { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public User? User { get; set; }
    public List<Product>? Items { get; set; } = []; // just the product GUID for now
}