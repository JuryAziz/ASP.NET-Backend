namespace Store.Dtos;

public class DeleteUserDto() {
    public Guid UserId { get; set; }
    public required string Email { get; set; }
}