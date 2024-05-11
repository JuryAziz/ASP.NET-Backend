using AutoMapper;
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Dtos;
using Store.EntityFramework;
using Store.Helpers;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/addresses")]
public class AddressesController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly AddressService _addressService = new (appDbContext, mapper);

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] int page = 1, [FromQuery] int limit = 25)
    {
        IEnumerable<AddressDto>? addresses = await _addressService.GetAddresses();
        IEnumerable<AddressDto> paginatedAddresses = Paginate.Function(addresses.ToList(), page, limit);
        return Ok(new BaseResponseList<AddressDto>(paginatedAddresses, true));
    }

    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddressById(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? foundAddress = await _addressService.GetAddressById(addressIdGuid);
        if (foundAddress is null) return NotFound();

        return Ok(new BaseResponse<AddressDto>(foundAddress, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(CreateAddressDto newAddress)
    {
        AddressDto? createdAddress = await _addressService.CreateAddress(newAddress);
        return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
    }

    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, UpdateAddressDto rawUpdatedAddress)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? addressToUpdate = await _addressService.GetAddressById(addressIdGuid);
        if (addressToUpdate is null) return NotFound();
        AddressDto? updatedAddress = await _addressService.UpdateAddress(addressIdGuid, rawUpdatedAddress);

        return Ok(new BaseResponse<AddressDto>(updatedAddress, true));
    }

    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? addressToDelete = await _addressService.GetAddressById(addressIdGuid);
        if (addressToDelete is null) return NotFound();
        DeleteAddressDto? deletedAddress = await _addressService.DeleteAddress(addressIdGuid);
        if (deletedAddress is null) return NotFound();

        return Ok(new BaseResponse<DeleteAddressDto>(deletedAddress, true));
    }
}