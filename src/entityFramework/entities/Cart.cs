using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;
[Table("Cart")]
public class Cart
{
    public Guid CartId { get; set; }
    public required Guid UserId { get; set; }
    public List<CartItem>? Items { get; set; } = [];
    public User? User { get; set; }
}
