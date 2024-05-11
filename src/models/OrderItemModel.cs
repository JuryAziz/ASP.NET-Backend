using System.ComponentModel.DataAnnotations;

namespace Store.Models;
public class OrderItemModel
{
    [Required(ErrorMessage = "Order Item Id is required.")]
    public required Guid OrderItemId { get; set; }

    [Required(ErrorMessage = "Order Id is required.")]
    public required Guid OrderId { get; set; }

    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(1.0, 10000.0, ErrorMessage = "Price must be between 1.0 and 10000.0.")]
    public required float Price { get; set; }

    [Required(ErrorMessage = "Quantity is required.")]
    [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
    public required int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}