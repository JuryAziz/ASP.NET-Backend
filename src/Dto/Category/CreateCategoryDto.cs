using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Store.Models;

namespace Store.Dto.Category;

public class CreateCategoryDto
{

    [Required(ErrorMessage = "Name Required Error msg.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name StringLength Error msg.")]
    public required string Name { get; set; }

    [StringLength(50, MinimumLength = 1, ErrorMessage = "Title StringLength Error msg.")]
    public string Description { get; set; } = string.Empty;
}