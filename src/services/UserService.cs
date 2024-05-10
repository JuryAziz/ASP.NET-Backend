using Microsoft.EntityFrameworkCore;
using Store.EntityFramework;

using Store.EntityFramework.Entities;
using Store.Models;

namespace Store.Application.Services;
public class UserService(AppDbContext appDbContext)
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<List<User>> GetUsers()
    {
        return await _appDbContext.Users
            .Include(user => user.Addresses)
            .ToListAsync();
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        return await Task.FromResult(
            await _appDbContext.Users
                .Include(user => user.Addresses)
                .FirstOrDefaultAsync(user => user.UserId == userId)
        );
    }

    public async Task<User?> CreateUser(UserModel newUser)
    {
        var user = new User
        {
            Email = newUser.Email,
            PhoneNumber = newUser.PhoneNumber,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            DateOfBirth = newUser.DateOfBirth,
            Role = newUser.Role,
            CreatedAt = newUser.CreatedAt
        };

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();
            
        return await Task.FromResult(user);
    }

    public async Task<User?> UpdateUser(Guid userId, UserModel updatedUser)
    {
        var userToUpdate = await GetUserById(userId);
        if (userToUpdate != null)
        {
            userToUpdate.Email = updatedUser.Email;
            userToUpdate.PhoneNumber = updatedUser.PhoneNumber;
            userToUpdate.FirstName = updatedUser.FirstName;
            userToUpdate.LastName = updatedUser.LastName;
            userToUpdate.DateOfBirth = updatedUser.DateOfBirth;
            userToUpdate.Role = updatedUser.Role;

            await _appDbContext.SaveChangesAsync();
        };
        
        return await Task.FromResult(userToUpdate);
    }

    public async Task<bool> DeleteUser(Guid userId)
    {
        var userToDelete = await GetUserById(userId);
        if (userToDelete == null) return await Task.FromResult(false);

        _appDbContext.Users.Remove(userToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(true);
    }
}
