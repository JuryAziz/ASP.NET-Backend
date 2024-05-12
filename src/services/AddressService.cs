using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;

using Store.EntityFramework.Entities;

namespace Store.Application.Services;
public class AddressService(AppDbContext appDbContext, IMapper mapper)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<AddressDto>> GetAddresses()
    {
        return (await _appDbContext.Addresses
            .ToListAsync()).Select(_mapper.Map<AddressDto>);
    }

    public async Task<AddressDto?> GetAddressById(Guid addressId)
    {
        return await Task.FromResult((await GetAddresses()).FirstOrDefault(a => a.AddressId == addressId));
    }

    public async Task<AddressDto?> CreateAddress(CreateAddressDto newAddress)
    {
        Address address = new () {
            UserId = newAddress.UserId,
            Country = newAddress.Country,
            State = newAddress.State,
            City = newAddress.City,
            Address1 = newAddress.Address1,
            Address2 = newAddress.Address2,
            PostalCode = newAddress.PostalCode,
            IsDefault = false,
            CreatedAt = DateTime.UtcNow
        };

        await _appDbContext.Addresses.AddAsync(address);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<AddressDto>(address));
    }

    public async Task<AddressDto?> UpdateAddress(Guid addressId, UpdateAddressDto updatedAddress)
    {
        AddressDto? addressToUpdate = await GetAddressById(addressId);
        if (addressToUpdate != null)
        {
            addressToUpdate.Country = updatedAddress.Country;
            addressToUpdate.State = updatedAddress.State;
            addressToUpdate.City = updatedAddress.City;
            addressToUpdate.Address1 = updatedAddress.Address1;
            addressToUpdate.Address2 = updatedAddress.Address2;
            addressToUpdate.PostalCode = updatedAddress.PostalCode;
            addressToUpdate.IsDefault = updatedAddress.IsDefault;

            await _appDbContext.SaveChangesAsync();
        };

        return await Task.FromResult(_mapper.Map<AddressDto>(addressToUpdate));
    }

    public async Task<DeleteAddressDto?> DeleteAddress(Guid addressId)
    {
        Address? addressToDelete = await _appDbContext.Addresses.FirstOrDefaultAsync(address => address.AddressId == addressId); 
        if (addressToDelete == null) return null;
        _appDbContext.Addresses.Remove(addressToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<DeleteAddressDto>(addressToDelete));
    }
}
