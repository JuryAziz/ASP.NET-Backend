namespace Store.Models.ProductReview;

public class ProductReview 
{
    public required Guid ReviewId { get; set;}
    public required Guid UserId { get; set; }
    public required Guid OrderId { get; set; }
    public required Guid ProductId { get; set; }
    public required int Rating { get; set; }
    public required string Title { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}