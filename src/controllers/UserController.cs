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
    public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 20)
    {
        try
        {
            if (limit > 20) limit = 20;
            List<User> users = await _userService.GetUsers(page, limit > 20 ? 20 : limit);
            return Ok(new BaseResponseList<User>(users, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetUsers' page {page} limit {limit}");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest(new BaseResponse<object>(false, "Invalid User ID Format"));

            User? foundUser = await _userService.GetUserById(userIdGuid);
            if (foundUser == null) return NotFound();

            return Ok(new BaseResponse<User>(foundUser, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetUserById'");
            return StatusCode(500, ex.Message );
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserModel newUser)
    {
        try
        {
            User? createdUser = await _userService.CreateUser(newUser);
            return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreateUser'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateUser(string userId, UserModel rawUpdatedUser)
    {
        try
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

            User? userToUpdate = await _userService.GetUserById(userIdGuid);
            if (userToUpdate == null) return BadRequest(ModelState);
            User? updatedUser = await _userService.UpdateUser(userIdGuid, rawUpdatedUser);

            return Ok(new BaseResponse<User>(updatedUser, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'UpdateUser'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        try
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");

            User? userToDelete = await _userService.GetUserById(userIdGuid);
            if (userToDelete == null || !await _userService.DeleteUser(userIdGuid)) return NotFound();

            return Ok(new BaseResponse<User>(userToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteUser'");
            return StatusCode(500, ex.Message);
        }
    }
}