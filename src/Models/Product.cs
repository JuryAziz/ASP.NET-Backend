using System.ComponentModel.DataAnnotations;

namespace Store.Models.Product;

public class Product
{
    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }

    [Required(ErrorMessage = "Category Id is required.")]
    public required Guid CategoryId { get; set; }

    [Required(ErrorMessage = "Merchant Id is required.")]
    public required Guid MerchantId { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at least 20 characters long.")]
    public required string Name { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Price is required.")]
    [Range(1.0, 10000.0, ErrorMessage = "Price must be between 1.0 and 10000.0.")]
    public required float Price { get; set; }

    [Range(0.0, 10000.0, ErrorMessage = "Discount must be between 1.0 and 10000.0.")]
    public float Discount { get; set; }

    [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
    public int Stock { get; set; }

    [Range(0, 10000, ErrorMessage = "Sold must be between 1 and 10000")]
    public int Sold { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}

