using Microsoft.AspNetCore.Mvc;

using Store.Application.Services.Addresses;

using Store.Models.Address;

using Store.Helpers.Reponses;

namespace Store.API.Controllers.Addresses
{
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
                var addresses = await _addressController.GetAddresses(page, limit);
                return Ok(new SuccessResponse<IEnumerable<Address>>() { Message = $"Addresses fetched! page: {page}  limit: {limit}", Data = addresses });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetPaymentMethods' page {page} limit {limit}");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> GetAddressById(string addressId)
        {
            try
            {
                if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var foundAddress = await _addressController.GetAddressById(addressIdGuid);
                if (foundAddress == null) return StatusCode(404, new SuccessResponse<string>() { Data = "Not Found" });
                return Ok(new SuccessResponse<Address>() { Message = $"PaymentMethod found with Guid {foundAddress.AddressId}", Data = foundAddress });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUserById'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(Address newAddress)
        {
            try
            {
                var createdAddress = await _addressController.CreateAddress(newAddress);
                return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'CreateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPut("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> UpdateAddress(string addressId, Address address)
        {
            try
            {
                if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var addressToUpdate = await _addressController.GetAddressById(addressIdGuid);
                if (addressToUpdate == null) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                await _addressController.UpdateAddress(addressIdGuid, address);
                return Ok(new SuccessResponse<Address>() { Message = $"User updated with guid of {addressToUpdate.AddressId}", Data = addressToUpdate });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'UpdateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpDelete("{addressId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> DeleteAddress(string addressId)
        {
            try
            {
                if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var addressToDelete = await _addressController.GetAddressById(addressIdGuid);
                if (addressToDelete == null || !await _addressController.DeleteAddress(addressIdGuid)) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                return Ok(new SuccessResponse<Address>() { Message = $"User deleted with the guid of {addressToDelete.AddressId}", Data = addressToDelete });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'DeleteUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

    }
}