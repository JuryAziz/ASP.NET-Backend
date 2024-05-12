namespace Store.Dtos;

public class DeleteAddressDto() {
    public required Guid AddressId { get; set; }
    public required Guid UserId { get; set; }
}