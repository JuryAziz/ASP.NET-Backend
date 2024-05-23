namespace Store.Dtos;

public class DeleteUserDto() {
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
}