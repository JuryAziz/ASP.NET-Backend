using System.ComponentModel.DataAnnotations;

namespace Store.Models;

public class CartModel
{
    [Required(ErrorMessage = "Cart Id is required.")]
    public required Guid CartId { get; set; }

    [Required(ErrorMessage = "User Id is required.")]
    public required Guid UserId { get; set; }
    public List<Guid> Items { get; set; } = [];
}