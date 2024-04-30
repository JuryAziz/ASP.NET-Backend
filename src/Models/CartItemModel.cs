namespace Store.Models;

public class CartItemModel
{
    public required Guid CartItemId { get; set; }
    public required Guid Item { get; set; }
    public required int Quantity { get; set; }
}