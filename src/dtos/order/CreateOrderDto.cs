using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

public class CreateOrderDto()
{
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
}

