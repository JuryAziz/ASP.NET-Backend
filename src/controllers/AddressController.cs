using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Addresses;
using Store.Models.Address;

namespace Store.API.Controllers.Addresses
{
    [ApiController]
    [Route("/api/addresses")]
    public class AddressesController(AddressService addressService) : ControllerBase
    {
        private readonly AddressService _addressController = addressService;

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            return Ok(await _addressController.GetAddresses());
        }

        [HttpGet("{addressId:guid}")]
        public async Task<IActionResult> GetAddressById(Guid addressId)
        {
            var foundUser = await _addressController.GetAddressById(addressId);
            if (foundUser == null) return NotFound();
            return Ok(foundUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddress(Address newAddress)
        {
            var createdAddress = await _addressController.CreateAddress(newAddress);
            return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
        }

        [HttpPut("{addressId:guid}")]
        public async Task<IActionResult> UpdateAddress(Guid addressId, Address address)
        {
            var addressToUpdate = await _addressController.GetAddressById(addressId);
            if (addressToUpdate == null) return NotFound();
            await _addressController.UpdateAddress(addressId, address);
            return Ok(addressToUpdate);
        }

        [HttpDelete("{addressId:guid}")]
        public async Task<IActionResult> DeleteAddress(Guid addressId)
        {
            var addressToDelete = await _addressController.GetAddressById(addressId);
            if (addressToDelete == null) return NotFound();
            if (!await _addressController.DeleteAddress(addressId)) return NotFound();
            return Ok($"Address with the ID of ({addressId}) was deleted!");

        }
    }
}