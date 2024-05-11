using System.ComponentModel.DataAnnotations;

namespace Store.Models;
public class ProductReviewModel
{
    [Required(ErrorMessage = "Review Id is required.")]
    public required Guid ReviewId { get; set;}

    [Required(ErrorMessage = "User Id is required.")]
    public required Guid UserId { get; set; }

    [Required(ErrorMessage = "Order Item Id is required.")]
    public required Guid OrderItemId { get; set; }

    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }

    [Required(ErrorMessage = "Rating is required.")]
    public required int Rating { get; set; }

    [Required(ErrorMessage = "Title Id is required.")]
    public required string Title { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string Description { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}