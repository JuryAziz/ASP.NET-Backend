using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

[Table("ProductReviews")]
public class ProductReview
{
    [Key]
    public required Guid ReviewId { get; set; }
    public required Guid UserId { get; set; } // foreign key of User
    // public User User { get; set; } // added navigation property
    public required Guid OrderId { get; set; } // foreign key of Order
    // public Order Order { get; set; } // added navigation property
    public required Guid ProductId { get; set; } // foreign key of Product
    // public Product Product { get; set; } // added navigation property 
    public required int Rating { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
