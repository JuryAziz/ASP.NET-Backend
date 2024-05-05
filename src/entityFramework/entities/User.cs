using System.ComponentModel.DataAnnotations.Schema;
using Store.EntityFramework.Entities;

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




/******************************************************************************/
    // ? Those lists I'm not sure if they are supposed to lists of GUIDs or the entities themselves!!
    // ? So I made them lists of Guids, it can be altered as needed!
    // * Answer: it's both like the cart one

    public Guid? CartId { get; set; }
    public Cart? Cart { get; set; }

    public List<Guid>? Addresses { get; set; }

    public List<Guid>? PaymentMethods { get; set; }
    public List<Guid>? ShoppingLists { get; set; }
    public List<Guid>? Orders { get; set; }
}