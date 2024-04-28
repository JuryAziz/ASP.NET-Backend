namespace Store.Models.User;

public class User
{
    public required Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public required string FirstName { get; set; }
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Role { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}