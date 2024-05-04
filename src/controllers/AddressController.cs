using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/addresses")]
public class AddressesController(AddressService addressService) : ControllerBase
{
    private readonly AddressService _addressController = addressService;

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            IEnumerable<AddressModel>? addresses = await _addressController.GetAddresses(page, limit);
            return Ok(new BaseResponseList<AddressModel>(addresses, true));
        }
        catch (Exception ex)
        {
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
            AddressModel? foundAddress = await _addressController.GetAddressById(addressIdGuid);
            if (foundAddress is null) return NotFound();
            return Ok(new BaseResponse<AddressModel>(foundAddress, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetUserById'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateAddress(AddressModel newAddress)
    {
        try
        {
            AddressModel? createdAddress = await _addressController.CreateAddress(newAddress);
            return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while 'CreateUser'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPut("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateAddress(string addressId, AddressModel address)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));
            AddressModel? addressToUpdate = await _addressController.GetAddressById(addressIdGuid);
            if (addressToUpdate is null) return NotFound();
            await _addressController.UpdateAddress(addressIdGuid, address);
            return Ok(new BaseResponse<AddressModel>(addressToUpdate, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateUser'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpDelete("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        try
        {
            if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));
            AddressModel? addressToDelete = await _addressController.GetAddressById(addressIdGuid);
            if (addressToDelete is null || !await _addressController.DeleteAddress(addressIdGuid)) return NotFound();
            return Ok(new BaseResponse<AddressModel>(addressToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteUser'");
            return StatusCode(500, ex.Message );
        }
    }

}