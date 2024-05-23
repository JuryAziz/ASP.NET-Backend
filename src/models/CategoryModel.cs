using System.ComponentModel.DataAnnotations;

namespace Store.Models;

public class CategoryModel
{
    public Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; } = string.Empty;
}