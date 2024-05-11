using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;
public class UpdateAddressDto()
{
    [Required(ErrorMessage = "Country is required.")]
    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public required string Country { get; set; }

    [Required(ErrorMessage = "State is required.")]
    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public required string State { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public required string City { get; set; }

    [Required(ErrorMessage = "Address 1 is required.")]
    public required string Address1 { get; set; }
    public string Address2 { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal Code is required.")]
    [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Invalid postal code format")]
    public required int PostalCode { get; set; }
    public bool IsDefault { get; set; } = true;
}