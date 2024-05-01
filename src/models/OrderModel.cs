using System.ComponentModel.DataAnnotations;

namespace Store.Models;

public class OrderModel
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

    [Required(ErrorMessage = "Order Id is required.")]
    public required Guid OrderId { get; set; }

    [Required(ErrorMessage = "User Id is required.")]
    public required Guid UserId { get; set; }

    [Required(ErrorMessage = "Address Id is required.")]
    public required Guid AddressId { get; set; }

    [Required(ErrorMessage = "Payment Method Id is required.")]
    public required Guid PaymentMethodId { get; set; }

    [Required(ErrorMessage = "Transaction Id is required.")]
    public required Guid TransactionId { get; set; }

    [Required(ErrorMessage = "Shipment Id is required.")]
    public required Guid ShipmentId { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public required OrderStatus Status { get; set; }

    public List<Guid> OrderItems { get; set; } = [];

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}