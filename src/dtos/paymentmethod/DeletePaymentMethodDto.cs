namespace Store.Dtos;

public class DeletePaymentMethodDto()
{
    public Guid PaymentMethodId { get; set; }
    public Guid UserId { get; set; }
}