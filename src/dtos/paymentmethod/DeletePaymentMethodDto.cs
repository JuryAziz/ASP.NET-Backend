namespace Store.Dtos;

public class DeletePaymentMethodDto()
{
    public required Guid PaymentMethodId { get; set; }
    public required Guid UserId { get; set; }
}