using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;
[Table("CartItem")]
public class CartItem
{   
     public Guid CartItemId { get; set; }
    public Guid? CartId { get; set; }
    public required Guid ProductId { get; set; }
    
    public required int Quantity { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual Cart? Cart { get; set; }
    public virtual Product? Product { get; set; }
}
