using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Store.entityFramework;

[Table("OrderItem")]
public class OrderItem
{
    public Guid OrderItemId { get; set; }

    // public Guid? OrderId { get; set; }

    // public required Guid ProductId { get; set; }

    public required float Price { get; set; }

    public required int Quantity { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}