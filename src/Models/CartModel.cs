namespace Store.Models;

public class CartModel
{
    public required Guid CartId { get; set; }
    public required Guid UserId { get; set; }
    public List<Guid> Items { get; set; } = [];
}