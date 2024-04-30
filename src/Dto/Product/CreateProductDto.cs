using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Store.Dto.Product;

public class CreateProductDto
{
  [Required(ErrorMessage = "Title Required Error msg.")]
  [StringLength(50, MinimumLength = 1, ErrorMessage = "Title StringLength Error msg.")]
  public required string Title { get; set; }


  [Required(ErrorMessage = "Price Required Error msg.")]
  [Range(0.0, Double.PositiveInfinity, ErrorMessage = "Price Range Error msg.")]
  public required decimal Price { get; set; }

  [Required(ErrorMessage = "TotalQuantity Required Error msg.")]
  [Range(0.0, int.MaxValue, ErrorMessage = "TotalQuantity Range Error msg.")]
  public required int TotalQuantity { get; set; }

  [StringLength(50, MinimumLength = 1, ErrorMessage = "Description StringLength Error msg.")]
  public required string Description { get; set; }

  [StringLength(50, MinimumLength = 1, ErrorMessage = "Thumbnail StringLength Error msg.")]
  [JsonIgnore]
  public string? Thumbnail { get; set; }

}