using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

public class UpdateOrderDto()
{
    public Guid AddressId { get; set; }

    public Guid PaymentMethodId { get; set; }

    public Guid TransactionId { get; set; }

    public Guid ShipmentId { get; set; }

    public int Status { get; set; }
}