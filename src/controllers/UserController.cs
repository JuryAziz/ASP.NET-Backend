using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Store.Helpers.Enums;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/users")]
public class UsersController(AppDbContext appDbContext, IPasswordHasher<User> passwordHasher, IMapper mapper) : ControllerBase
{
    private readonly UserService _userService = new (appDbContext, mapper, passwordHasher);
    private readonly AuthSerivce _authService = new (appDbContext, mapper);

    #pragma warning disable CS8604 // Possible null reference argument.

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        IEnumerable<UserDto> users = await _userService.GetUsers();

        IEnumerable<UserDto> sortedUsers = users;
        switch (dir.ToLower())
        {
            case "asc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedUsers = sortedUsers.OrderBy(u => u.FirstName).ToList();
                        break;
                    // case "orders":
                    //     sortedUsers = sortedUsers.OrderBy(u => u.Orders?.Count).ToList();
                    //     break;
                    case "email":
                        sortedUsers = sortedUsers.OrderBy(u => u.Email).ToList();
                        break;
                    case "createdat":
                        sortedUsers = sortedUsers.OrderBy(u => u.CreatedAt).ToList();
                        break;
                }
                break;
            case "desc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.FirstName).ToList();
                        break;
                    // case "orders":
                    //     sortedUsers = sortedUsers.OrderByDescending(u => u.Orders?.Count).ToList();
                    //     break;
                    case "email":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "createdat":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.CreatedAt).ToList();
                        break;
                }
                break;
        }

        PaginationResult<UserDto> paginatedUsers = new() { Items = sortedUsers.Skip((pageNumber - 1) * pageSize).Take(pageSize), TotalCount = sortedUsers.Count(), PageNumber = pageNumber, PageSize = pageSize };
        return Ok(new BaseResponse<PaginationResult<UserDto>>(paginatedUsers, true));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid User ID Format"));

        User? foundUser = await _userService.GetUserById(userIdGuid);
        if (foundUser is null) return NotFound();

        return Ok(new BaseResponse<User>(foundUser, true));
    }

    [Authorize]
    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, UpdateUserDto rawUpdatedUser)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToUpdate = await _userService.GetUserById(userIdGuid);
        if (userToUpdate is null) return BadRequest(ModelState);
        UserDto? updatedUser = await _userService.UpdateUser(userIdGuid, rawUpdatedUser);

        return Ok(new BaseResponse<UserDto>(updatedUser, true));
    }

    [Authorize]
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var userIdString = _authService.Authenticate(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, UserRole.Admin);
        if(userIdString != null) return Unauthorized(new BaseResponse<string>(false, userIdString));

        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToDelete = await _userService.GetUserById(userIdGuid);
        if (userToDelete is null) return NotFound();
        DeleteUserDto? deletedUser = await _userService.DeleteUser(userIdGuid);
        if (deletedUser is null) return NotFound();

        return Ok(new BaseResponse<DeleteUserDto>(deletedUser, true));
    }
}
