using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

public class CreateProductDto()
{
    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(50, ErrorMessage = "Name must be at most 30 characters long.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(1.0, 100000000.0, ErrorMessage = "Price must be between 1.0 and 10000.0.")]
    public required double Price { get; set; }

    [Range(0, 1000000, ErrorMessage = "Stock must be between 1 and 10000")]
    public required int Stock { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string? Description { get; set; }
    public string? Thumbnail { get; set; }
    public string[] Images { get; set; } = [];

    public string? Category { get; set; }
}

