using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("Users")]
public class User
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Role { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;


    public List<Address>? Addresses { get; set; }
    
    // public Cart Cart { get; set;} = new Cart();
    // public List<PaymentMethod>? PaymentMethods { get; set; }
    // public List<ShoppingList>? ShoppingLists { get; set; }
    // public List<Order>? Orders { get; set; }
}