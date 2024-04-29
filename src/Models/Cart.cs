namespace Store.Models.Cart;

public class Cart
{
    public required Guid CartId { get; set; }
    public required Guid UserId { get; set; }
    public List<Guid> Items { get; set; } = [];
}