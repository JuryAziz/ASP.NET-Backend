namespace Store.Dtos;

public class DeleteProductDto()
{
    public required Guid ProductId { get; set; }
    public required string Name { get; set; }
}