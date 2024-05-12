namespace Store.Dtos;

public class AddressDto()
{
    public required Guid AddressId { get; set; }
    public required string Country { get; set; }
    public required string State { get; set; }
    public required string City { get; set; }
    public required string Address1 { get; set; }
    public string Address2 { get; set; } = string.Empty;
    public required int PostalCode { get; set; }
    public bool IsDefault { get; set; } = true;
}