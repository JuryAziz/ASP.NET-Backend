using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Dtos;
using Store.EntityFramework;
using Store.Helpers;
using Store.Helpers.Enums;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/addresses")]
public class AddressesController(AppDbContext appDbContext, IMapper mapper) : ControllerBase
{
    private readonly AddressService _addressService = new (appDbContext, mapper);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

    [HttpGet]
    public async Task<IActionResult> GetAddresses([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 25)
    {
        IEnumerable<AddressDto>? addresses = await _addressService.GetAddresses();
        PaginationResult<AddressDto> paginatedAddresses = new() { Items = addresses.Skip((pageNumber - 1) * pageSize).Take(pageSize), TotalCount = addresses.Count(), PageNumber = pageNumber, PageSize = pageSize };
        return Ok(new BaseResponse<PaginationResult<AddressDto>>(paginatedAddresses, true));
    }

    [HttpGet("{addressId}")]
    public async Task<IActionResult> GetAddressById(string addressId)
    {
        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? foundAddress = await _addressService.GetAddressById(addressIdGuid);
        if (foundAddress is null) return NotFound();

        return Ok(new BaseResponse<AddressDto>(foundAddress, true));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateAddress(CreateAddressDto newAddress)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        AddressDto? createdAddress = await _addressService.CreateAddress(newAddress);
        return CreatedAtAction(nameof(GetAddressById), new { createdAddress?.AddressId }, createdAddress);
    }

    [Authorize]
    [HttpPut("{addressId}")]
    public async Task<IActionResult> UpdateAddress(string addressId, UpdateAddressDto rawUpdatedAddress)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? addressToUpdate = await _addressService.GetAddressById(addressIdGuid);
        if (addressToUpdate is null) return NotFound();
        AddressDto? updatedAddress = await _addressService.UpdateAddress(addressIdGuid, rawUpdatedAddress);

        return Ok(new BaseResponse<AddressDto>(updatedAddress, true));
    }

    [Authorize]
    [HttpDelete("{addressId}")]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(addressId, out Guid addressIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid Address ID Format"));

        AddressDto? addressToDelete = await _addressService.GetAddressById(addressIdGuid);
        if (addressToDelete is null) return NotFound();
        DeleteAddressDto? deletedAddress = await _addressService.DeleteAddress(addressIdGuid);
        if (deletedAddress is null) return NotFound();

        return Ok(new BaseResponse<DeleteAddressDto>(deletedAddress, true));
    }
}