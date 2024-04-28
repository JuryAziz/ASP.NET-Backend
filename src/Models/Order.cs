namespace Store.Models.Order;
public class Order
{
    public required Guid OrderId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid AddressId { get; set; }
    public required Guid PaymentMethodId { get; set; }
    public required Guid TransactionId { get; set; }
    public required Guid ShipmentId { get; set; }
    public required int Status { get; set; }
    public List<Guid> OrderItems { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}