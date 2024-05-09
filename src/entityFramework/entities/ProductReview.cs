using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Store.EntityFramework.Entities;

[Table("ProductReviews")]
public class ProductReview
{
    public Guid ReviewId { get; set; }
    public Guid UserId { get; set; } // foreign key of User
    public User User { get; set; } // added navigation property
    public Guid OrderId { get; set; } // foreign key of Order
    public Order Order { get; set; } // added navigation property
    public Guid ProductId { get; set; } // foreign key of Product
    public Product Product { get; set; } // added navigation property 
    public int Rating { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
