using System.ComponentModel.DataAnnotations;
using Store.EntityFramework.Entities;

namespace Store.Models;
public class ShoppingListModel
{
    [Required(ErrorMessage = "Shopping list Id is required.")]
    public required Guid ShoppingListId { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public required Guid UserId { get; set; }

    [Required(ErrorMessage = "List name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public required string Name { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string Description { get; set; } = string.Empty;
    public virtual List<Product>? Items { get; set; } = [];
    public bool IsPublic { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}