using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Store.EntityFramework;

[Table("PaymentMethod")]
public class PaymentMethod
{
    public required Guid PaymentMethodId { get; set; }
    public required Guid UserId { get; set; }
    
    public required string Type { get; set; }
    public required decimal CardNumber { get; set; }
    public required string CardHolderName { get; set; }
    public required DateTime CardExpirationDate { get; set; }
    public required int CardCCV { get; set; }
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual List<Order>? Orders { get; set; }
}