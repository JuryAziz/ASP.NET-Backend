using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("ProductReview")]
public class ProductReview
{
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; } // foreign key of User
    public Guid OrderId { get; set; } // foreign key of Order
    public Guid ProductId { get; set; } // foreign key of Product

    public int Rating { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


    public virtual Order? Order { get; set; } // added navigation property
    public virtual Product? Product { get; set; } // added navigation property 
    public virtual User? User { get; set; } // added navigation property

}
