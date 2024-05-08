using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Store.Application.Services;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/addresses")]
public class AddressesController(AppDbContext appDbContext) : ControllerBase
{
    private readonly AddressService _addressService = new (appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] int page = 1, [FromQuery] int limit = 25)
    {
        List<Address>? addresses = await _addressService.GetAddresses();
        List<Address> paginatedAddresses = Paginate.Function(addresses, page, limit);
        return Ok(new BaseResponseList<Address>(paginatedAddresses, true));
    }

    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddressById(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return NotFound(new BaseResponse<object>(false, "Invalid Address ID Format"));

        Address? foundAddress = await _addressService.GetAddressById(addressIdGuid);
        if (foundAddress is null) return NotFound(new BaseResponse<object>(false, "No address found with that ID"));

        return Ok(new BaseResponse<Address>(foundAddress, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(AddressModel newAddress)
    {
        Address? createdAddress = await _addressService.CreateAddress(newAddress);
        return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
    }

    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, AddressModel rawUpdatedAddress)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        Address? addressToUpdate = await _addressService.GetAddressById(addressIdGuid);
        if (addressToUpdate is null) return NotFound();
        await _addressService.UpdateAddress(addressIdGuid, rawUpdatedAddress);

        return Ok(new BaseResponse<Address>(addressToUpdate, true));
    }

    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        Address? addressToDelete = await _addressService.GetAddressById(addressIdGuid);
        if (addressToDelete is null || !await _addressService.DeleteAddress(addressIdGuid)) return NotFound();

        return Ok(new BaseResponse<Address>(addressToDelete, true));
    }
}