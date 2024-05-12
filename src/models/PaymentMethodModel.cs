using System.ComponentModel.DataAnnotations;

namespace Store.Models;
public class PaymentMethodModel
{
    [Required(ErrorMessage = "Payment Method ID is required.")]
    public required Guid PaymentMethodId { get; set; }

    [Required(ErrorMessage = "User ID is required.")]
    public required Guid UserId { get; set; }

    [Required(ErrorMessage = "Type is required.")]
    [MaxLength(20, ErrorMessage = "Type can be at most 20 characters long.")]
    public required string Type { get; set; }

    [Required(ErrorMessage = "Card number is required.")]
    [MinLength(12, ErrorMessage = "Cardholder name can be at least 12 characters long.")]
    [MaxLength(16, ErrorMessage = "Cardholder name can be at most 16 characters long.")]
    public required string CardNumber { get; set; }

    [Required(ErrorMessage = "Cardholder name is required.")]
    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Cardholder name can only contain letters and spaces.")]
    [MaxLength(50, ErrorMessage = "Cardholder name can be at most 50 characters long.")]
    public required string CardHolderName { get; set; }

    [Required(ErrorMessage = "Expiration date is required.")]
    [CustomValidation(typeof(PaymentMethodModel), nameof(ValidateCardExpirationDate))]
    public required DateTime CardExpirationDate { get; set; }

    [Required(ErrorMessage = "CCV is required.")]
    [RegularExpression(@"^\d{3}$", ErrorMessage = "CCV must be exactly three digits.")]
    public required int CardCCV { get; set; }

    public bool IsDefault { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // method for card expire date
    public static ValidationResult? ValidateCardExpirationDate(DateTime expirationDate, ValidationContext context)
    {
        if (expirationDate <= DateTime.Today)
            return new ValidationResult("Expiration date must be in the future.");

        return ValidationResult.Success;
    }
}