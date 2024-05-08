using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("Users")]
public class User
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Role { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.Now;


/******************************************************************************/
    // Those lists I'm not usre if they are supposed to lists of GUIDs or the entities themselves!!
    // So I made them lists of Guids, it can be altered as needed!


    public virtual Guid? Cart { get; set;}

    public virtual List<Address>? Addresses { get; set; }

    // public List<PaymentMethod>? PaymentMethods { get; set; }
    // public List<ShoppingList>? ShoppingLists { get; set; }
    // public List<Order>? Orders { get; set; }
}