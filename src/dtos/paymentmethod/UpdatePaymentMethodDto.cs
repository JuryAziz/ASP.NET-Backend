using System.ComponentModel.DataAnnotations;
using Store.Models;

namespace Store.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class UpdatePaymentMethodDto()
{
    [MaxLength(20, ErrorMessage = "Type can be at most 20 characters long.")]
    public string Type { get; set; }

    [Range(100000000000, 9999999999999999, ErrorMessage = "Card number must be between 12 to 16 digits")]
    public double CardNumber { get; set; }

    [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Cardholder name can only contain letters and spaces.")]
    [MaxLength(50, ErrorMessage = "Cardholder name can be at most 50 characters long.")]
    public string CardHolderName { get; set; }

    [CustomValidation(typeof(PaymentMethodModel), nameof(ValidateCardExpirationDate))]
    public DateTime CardExpirationDate { get; set; }

    [RegularExpression(@"^\d{3}$", ErrorMessage = "CCV must be exactly three digits.")]
    public int CardCCV { get; set; }

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
