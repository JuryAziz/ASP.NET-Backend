using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class UpdateUserDto()
{
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(100, ErrorMessage = "Email can be at most 100 characters long.")]
    public string Email { get; set; }

    [RegularExpression(@"^\(?\d{10}$", ErrorMessage = "Invalid phone number format.")]
    public string PhoneNumber { get; set; }

    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public string FirstName { get; set; }

    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public string? LastName { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; } = default;
    public int? Role { get; set; } = 0;
}