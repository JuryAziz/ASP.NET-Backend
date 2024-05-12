using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/users")]
public class UsersController(AppDbContext appDbContext) : ControllerBase
{
    private readonly UserService _userService = new(appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 50, [FromQuery] string sortBy = "Name", [FromQuery] string dir = "Asc")
    {
        List<User> users = await _userService.GetUsers();

        List<User> sortedUsers = users;
        switch (dir.ToLower())
        {
            case "asc":
                switch (sortBy.ToLower())
                {
                    case "name":
                        sortedUsers = sortedUsers.OrderBy(u => u.FirstName).ToList();
                        break;
                    case "orders":
                        sortedUsers = sortedUsers.OrderBy(u => u.Orders?.Count).ToList();
                        break;
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
                    case "orders":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.Orders?.Count).ToList();
                        break;
                    case "email":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.Email).ToList();
                        break;
                    case "createdat":
                        sortedUsers = sortedUsers.OrderByDescending(u => u.CreatedAt).ToList();
                        break;
                }
                break;
        }

        List<User> paginatedUsers = Paginate.Function(sortedUsers, page, limit);
        return Ok(new BaseResponseList<User>(paginatedUsers, true));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid User ID Format"));

        User? foundUser = await _userService.GetUserById(userIdGuid);
        if (foundUser is null) return NotFound();

        return Ok(new BaseResponse<User>(foundUser, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserModel newUser)
    {
        User? createdUser = await _userService.CreateUser(newUser);
        return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(string userId, UserModel rawUpdatedUser)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToUpdate = await _userService.GetUserById(userIdGuid);
        if (userToUpdate is null) return BadRequest(ModelState);
        User? updatedUser = await _userService.UpdateUser(userIdGuid, rawUpdatedUser);

        return Ok(new BaseResponse<User>(updatedUser, true));
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToDelete = await _userService.GetUserById(userIdGuid);
        if (userToDelete is null || !await _userService.DeleteUser(userIdGuid)) return NotFound();

        return Ok(new BaseResponse<User>(userToDelete, true));
    }
}