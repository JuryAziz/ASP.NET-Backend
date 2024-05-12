using System.ComponentModel.DataAnnotations;

namespace Store.Models;
public class CartItemModel
{
    [Required(ErrorMessage = "Cart Id is required.")]
    public required Guid CartId { get; set; }

    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
    public required int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

}