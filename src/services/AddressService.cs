using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;
public class AddressService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<Address>> GetAddresses()
    {
        return await _appDbContext
            .Addresses
            .ToListAsync();
    }

    public async Task<List<Address>> GetUserAddresses(Guid userId)
    {
        return await Task.FromResult(
            (await _appDbContext.Addresses.ToListAsync())
                .FindAll(address => address.UserId == userId)
        );    
    }

    public async Task<Address?> GetAddressById(Guid addressId)
    {
        return await Task.FromResult(
            await _appDbContext.Addresses
                .FirstOrDefaultAsync(address => address.AddressId == addressId)
        );    
    }

    public async Task<Address?> CreateAddress(AddressModel newAddress)
    {
        var address = new Address {
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

        return await Task.FromResult(address);
    }

    public async Task<Address?> UpdateAddress(Guid addressId, AddressModel updatedAddress)
    {
        var addressToUpdate = await GetAddressById(addressId);
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

        return await Task.FromResult(addressToUpdate);
    }

    public async Task<bool> DeleteAddress(Guid addressId)
    {
        var addressToDelete = await GetAddressById(addressId);
        if (addressToDelete == null) return await Task.FromResult(false);
        
        _appDbContext.Addresses.Remove(addressToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
