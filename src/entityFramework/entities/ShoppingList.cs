using System.ComponentModel.DataAnnotations.Schema;

namespace Store.Models;

[Table("ShoppingLists")]
public class ShoppingList
{
    public required Guid ShoppingListId { get; set; }

    public required Guid UserId { get; set; }

    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public List<Guid>? Items { get; set; }

    public bool IsPublic { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}