using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Store.Dtos;
using Store.EntityFramework;

using Store.EntityFramework.Entities;

namespace Store.Application.Services;
public class UserService(AppDbContext appDbContext, IPasswordHasher<User> passwordHasher, IMapper mapper)
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
        #pragma warning disable CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
        #pragma warning disable CS8602 // Dereference of a possibly null reference.
        User? user = await _appDbContext.Users
            .Include(user => user.Cart)
                .ThenInclude(cart => cart.Items)
                .ThenInclude(cartItem => cartItem.Product)
            .Include(user => user.Addresses)
            .Include(user => user.PaymentMethods)
            .Include(user => user.Orders)
                .ThenInclude(order => order.Items)
                .ThenInclude(orderItem => orderItem.Product)
            // .Include(user => user.ShoppingLists)
            .Include(user => user.ProductReviews)
                .ThenInclude(productReview => productReview.Product)
            .Include(user => user.ProductReviews)
                .ThenInclude(productReview => productReview.OrderItem)
            .FirstOrDefaultAsync(user => user.UserId == userId);
        #pragma warning restore CS8602 // Dereference of a possibly null reference.
        #pragma warning restore CS8620 // Argument cannot be used for parameter due to differences in the nullability of reference types.
  
        return await Task.FromResult(user);
    }   

    public async Task<UserDto?> CreateUser(CreateUserDto newUser)
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

        UserDto DtoUser = _mapper.Map<UserDto>(user);

        return await Task.FromResult(DtoUser);
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

        DeleteUserDto? deletedUser = _mapper.Map<DeleteUserDto>(userToDelete);

        return await Task.FromResult(deletedUser);
    }

    public async Task<UserDto?> UserLogin(LoginDto loginDto) {
        User? foundUser = await _appDbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == loginDto.Email.ToLower()); 
        if(foundUser is null) return null;

        PasswordVerificationResult passwordVerified = _passwordHasher.VerifyHashedPassword(foundUser, foundUser.Password, loginDto.Password);
        if(passwordVerified is not PasswordVerificationResult.Success) return null;

        return await Task.FromResult(_mapper.Map<UserDto>(foundUser)); 
    } 
}
