namespace Store.Dtos;

public class DeleteOrderDto()
{
    public required Guid OrderId { get; set; }
    public required Guid UserId { get; set; }
}