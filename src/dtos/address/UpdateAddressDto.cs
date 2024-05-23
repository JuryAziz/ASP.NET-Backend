using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class UpdateAddressDto()
{
    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public string Country { get; set; }

    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public string State { get; set; }

    [MinLength(2, ErrorMessage = "Description must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Description can be at most 20 characters long.")]
    public string City { get; set; }

    public string Address1 { get; set; }
    public string Address2 { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9]{5}$", ErrorMessage = "Invalid postal code format")]
    public int PostalCode { get; set; }
    public bool IsDefault { get; set; } = true;
}