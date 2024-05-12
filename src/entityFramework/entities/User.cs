using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;
[Table("User")]
public class User
{
    public Guid UserId { get; set; }

    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public string? LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } = default;
    public int? Role { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual Cart? Cart { get; set;}
    public virtual List<Address>? Addresses { get; set; } = [];
    public virtual List<PaymentMethod>? PaymentMethods { get; set; } = [];
    public virtual List<Order>? Orders { get; set; } = [];
    public List<ShoppingList>? ShoppingLists { get; set; } = [];
    public virtual List<ProductReview>? ProductReviews { get; set; } = [];

}