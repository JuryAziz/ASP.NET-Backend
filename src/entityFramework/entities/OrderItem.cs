using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;
[Table("OrderItem")]
public class OrderItem
{
    public Guid OrderItemId { get; set; }
    public Guid? OrderId { get; set; }
    public required Guid ProductId { get; set; }
    
    public required float Price { get; set; }
    public required int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual Order? Order { get; set; }
    public virtual Product? Product { get; set; }
    public virtual ProductReview? ProductReview { get; set; }
}