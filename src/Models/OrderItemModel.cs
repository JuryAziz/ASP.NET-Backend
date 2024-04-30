namespace Store.Models;

public class OrderItemModel
{
    public required Guid OrderItemId { get; set; }
    public required Guid OrderId { get; set; }
    public required Guid ProductId { get; set; }
    public required float Price { get; set; }
    public required int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}