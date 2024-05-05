using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Store.entityFramework;

[Table("Order")]
public class Order
{
    // ? needs discussion
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Completed,
        Cancelled
    }

    public required Guid OrderId { get; set; }

    public required Guid UserId { get; set; }

    public required Guid AddressId { get; set; }

    public required Guid PaymentMethodId { get; set; }

    public required Guid TransactionId { get; set; }

    public required Guid ShipmentId { get; set; }

    public required OrderStatus Status { get; set; }

    public List<Guid> OrderItems { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}