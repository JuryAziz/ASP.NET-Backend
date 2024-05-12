using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;
[Table("PaymentMethod")]
public class PaymentMethod
{
    public Guid PaymentMethodId { get; set; }
    public required Guid UserId { get; set; }
    
    public required string Type { get; set; }
    public required string CardNumber { get; set; }
    public required string CardHolderName { get; set; }
    public required DateTime CardExpirationDate { get; set; }
    public required int CardCCV { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual User? User { get; set; }
}