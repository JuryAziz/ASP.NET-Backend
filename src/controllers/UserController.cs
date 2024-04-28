using Microsoft.AspNetCore.Mvc;
using Store.Application.Services.Users;
using Store.Models.User;

namespace Store.API.Controllers.Users
{
    [ApiController]
    [Route("/api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController()
        {
            _userService = new UserService();
        }


        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var foundUser = await _userService.GetUserById(userId);
            if (foundUser == null) return NotFound();
            return Ok(foundUser);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User newUser)
        {
            var createdUser = await _userService.CreateUser(newUser);
            return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
        }

        [HttpPut("{userId:guid}")]
        public async Task<IActionResult> UpdateUser(Guid userId, User user)
        {
            var userToUpdate = await _userService.GetUserById(userId);
            if (userToUpdate == null) return NotFound();
            await _userService.UpdateUser(userId, userToUpdate);
            return Ok(userToUpdate);
        }

        [HttpDelete("{userId:guid}")]
        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var userToDelete = await _userService.GetUserById(userId);
            if (userToDelete == null) return NotFound();
            if (!await _userService.DeleteUser(userId)) return NotFound();
            return Ok($"User with the ID of ({userId}) was deleted!");

        }
    }
}