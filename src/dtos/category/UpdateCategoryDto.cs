using System.ComponentModel.DataAnnotations;

namespace Store.Dtos;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
public class UpdateCategoryDto()
{
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
    [MaxLength(20, ErrorMessage = "Name must be at most 20 characters long.")]
    public string Name { get; set; }

    [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
    [MaxLength(500, ErrorMessage = "Description can be at most 500 characters long.")]
    public string Description { get; set; } = string.Empty;
}