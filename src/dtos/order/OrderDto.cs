namespace Store.Dtos;

public class OrderDto()
{
    public required Guid UserId { get; set; }
    public required Guid AddressId { get; set; }
    public required Guid PaymentMethodId { get; set; }
    public required Guid TransactionId { get; set; }
    public required Guid ShipmentId { get; set; }
    public required int Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}