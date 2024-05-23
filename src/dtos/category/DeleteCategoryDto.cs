namespace Store.Dtos;

public class DeleteCategoryDto()
{
    public required Guid CategoryId { get; set; }
    public required string Name { get; set; }
}