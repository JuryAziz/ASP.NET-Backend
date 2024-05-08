using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> GetAddresses([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            List<Address>? addresses = await _addressService.GetAddresses(page, limit > 20 ? 20 : limit);
            return Ok(new BaseResponseList<Address>(addresses, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"An error occured while 'GetPaymentMethods' page {page} limit {limit}");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpGet("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetAddressById(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

            Address? foundAddress = await _addressService.GetAddressById(addressIdGuid);
            if (foundAddress is null) return NotFound();

            return Ok(new BaseResponse<Address>(foundAddress, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetAddressById'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(AddressModel newAddress)
    {
        try
        {
            Address? createdAddress = await _addressService.CreateAddress(newAddress);
            return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'CreateAddress'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPut("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateAddress(string addressId, AddressModel rawUpdatedAddress)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

            Address? addressToUpdate = await _addressService.GetAddressById(addressIdGuid);
            if (addressToUpdate is null) return NotFound();
            await _addressService.UpdateAddress(addressIdGuid, rawUpdatedAddress);

            return Ok(new BaseResponse<Address>(addressToUpdate, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateAddress'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpDelete("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

            Address? addressToDelete = await _addressService.GetAddressById(addressIdGuid);
            if (addressToDelete is null || !await _addressService.DeleteAddress(addressIdGuid)) return NotFound();

            return Ok(new BaseResponse<Address>(addressToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteAddress'");
            return StatusCode(500, ex.Message );
        }
    }
}