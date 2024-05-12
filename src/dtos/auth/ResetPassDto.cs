using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;
public class ResetPasswordDto 
{
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [MinLength(8, ErrorMessage = "Name must be at least 8 characters long.")]
    [MaxLength(16, ErrorMessage = "Name must be at most 16 characters long.")]
    [DefaultValue("MyNewPassword")]
    public string NewPassword { get; set; } = string.Empty;
}