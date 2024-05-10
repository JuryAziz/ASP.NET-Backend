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

    private readonly UserService _userService = new (appDbContext);

    [HttpGet]
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 50)
    {
        List<User> users = await _userService.GetUsers();
        List<User> paginatedUsers = Paginate.Function(users, page, limit);
        return Ok(new BaseResponseList<User>(paginatedUsers, true));
    }

    [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid User ID Format"));

        User? foundUser = await _userService.GetUserById(userIdGuid);
        if (foundUser == null) return NotFound();

        return Ok(new BaseResponse<User>(foundUser, true));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserModel newUser)
    {
        User? createdUser = await _userService.CreateUser(newUser);
        return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
    }

    [HttpPut("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateUser(string userId, UserModel rawUpdatedUser)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToUpdate = await _userService.GetUserById(userIdGuid);
        if (userToUpdate == null) return BadRequest(ModelState);
        User? updatedUser = await _userService.UpdateUser(userIdGuid, rawUpdatedUser);

        return Ok(new BaseResponse<User>(updatedUser, true));
    }

    [HttpDelete("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

        User? userToDelete = await _userService.GetUserById(userIdGuid);
        if (userToDelete == null || !await _userService.DeleteUser(userIdGuid)) return NotFound();

        return Ok(new BaseResponse<User>(userToDelete, true));
    }
}