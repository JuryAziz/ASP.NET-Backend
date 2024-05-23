using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;

using Store.EntityFramework.Entities;

namespace Store.Application.Services;
public class UserService(AppDbContext appDbContext, IMapper mapper, IPasswordHasher<User> passwordHasher)
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        return (await _appDbContext.Users
            .ToListAsync())
            .Select(_mapper.Map<UserDto>);
    }

    public async Task<User?> GetUserById(Guid userId)
    {
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        User? user = await _appDbContext.Users
            .Include(user => user.Cart)
                .ThenInclude(cart => cart.Items)
            .Include(user => user.Addresses)
            .Include(user => user.PaymentMethods)
            .Include(user => user.Orders)
            .Include(user => user.ShoppingLists)
            .Include(user => user.ProductReviews)
            .FirstOrDefaultAsync(user => user.UserId == userId);
        user.Password = null;
        return await Task.FromResult(user);
    }   

    public async Task<UserDto?> CreateUser(RegisterDto newUser)
    {
        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        User user = new () {
            Email = newUser.Email,
            Password = _passwordHasher.HashPassword(null, newUser.Password),
            PhoneNumber = newUser.PhoneNumber,
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            DateOfBirth = newUser.DateOfBirth,
            Role = newUser.Role,
            CreatedAt = newUser.CreatedAt
        };
        #pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

        await _appDbContext.Users.AddAsync(user);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<UserDto>(user));
    }

    public async Task<UserDto?> UpdateUser(Guid userId, UpdateUserDto updatedUser)
    {
        User? userToUpdate = await GetUserById(userId);
        if (userToUpdate is null) return null;

        userToUpdate.Email = updatedUser.Email;
        userToUpdate.PhoneNumber = updatedUser.PhoneNumber;
        userToUpdate.FirstName = updatedUser.FirstName;
        userToUpdate.LastName = updatedUser.LastName;
        userToUpdate.DateOfBirth = updatedUser.DateOfBirth;
        userToUpdate.Role = updatedUser.Role;

        await _appDbContext.SaveChangesAsync();
        
        return await Task.FromResult(_mapper.Map<UserDto>(userToUpdate));
    }

    public async Task<DeleteUserDto?> DeleteUser(Guid userId)
    {
        User? userToDelete = await _appDbContext.Users.FirstOrDefaultAsync(user => user.UserId == userId);
        if (userToDelete is null) return null;

        _appDbContext.Users.Remove(userToDelete);
        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(_mapper.Map<DeleteUserDto>(userToDelete));
    }
}
