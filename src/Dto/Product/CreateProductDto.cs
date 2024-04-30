using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store.Dto.Product;

public class CreateProductDto
{
  [Required(ErrorMessage = "Name is required.")]
  [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
  [MaxLength(20, ErrorMessage = "Name must be at least 20 characters long.")]
  public required string Name { get; set; }


  [Required(ErrorMessage = "Price is required.")]
  [Range(1.0, 10000.0, ErrorMessage = "Price must be between 1.0 and 10000.0.")]
  public required decimal Price { get; set; }

  [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
  public required int Stock { get; set; }

  [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
  [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
  public required string Description { get; set; }

}
/*

public class Product
{
    [Required(ErrorMessage = "Product Id is required.")]
    public required Guid ProductId { get; set; }
    
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


    [Range(0, 10000, ErrorMessage = "Stock must be between 1 and 10000")]
    public int Stock { get; set; }

}




**/