using Microsoft.AspNetCore.Mvc;

using Store.Application.Services.Users;
using Store.Application.Services.Addresses;
using Store.Application.Services.PaymentMethods;

using Store.Models.User;
using Store.Models.Address;
using Store.Models.PaymentMethod;

using Store.Helpers.Reponses;

namespace Store.API.Controllers.Users
{
    [ApiController]
    [Route("/api/users")]
    public class UsersController(UserService userService, AddressService addressService, PaymentMethodService paymentMethodService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly AddressService _addressService = addressService;
        private readonly PaymentMethodService _paymentMethodService = paymentMethodService;

        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] int page = 1, [FromQuery] int limit = 20)
        {
            try
            {
                if (limit > 20) limit = 20;
                var users = await _userService.GetUsers(page, limit);
                return Ok(new SuccessResponse<IEnumerable<User>>() { Message = $"Users fetched! page: {page}  limit: {limit}", Data = users });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUsers' page {page} limit {limit}");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var foundUser = await _userService.GetUserById(userIdGuid);
                if (foundUser == null) return StatusCode(404, new SuccessResponse<string>() { Data = "Not Found" });
                return Ok(new SuccessResponse<User>() { Message = $"User found with Guid {foundUser.UserId}", Data = foundUser });

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUserById'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}/addresses")]
        public async Task<IActionResult> GetUserAddresses(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var foundUserAddress = await _addressService.GetUserAddresses(userIdGuid);
                if (foundUserAddress == null) return StatusCode(404, new SuccessResponse<string>() { Data = "Not Found" });
                return Ok(new SuccessResponse<IEnumerable<Address>>() { Message = $"User address found belonding to Guid {userIdGuid}", Data = foundUserAddress });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUserAddresses'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}/paymentmethods")]
        public async Task<IActionResult> GetUserPaymentMethods(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var foundUserPaymentMethods = await _paymentMethodService.GetUserPaymentMethods(userIdGuid);
                if (foundUserPaymentMethods == null) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                return Ok(new SuccessResponse<IEnumerable<PaymentMethod>>() { Message = $"User payment methods found belonding to Guid {userIdGuid}", Data = foundUserPaymentMethods });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'GetUserPaymentMethods'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(User newUser)
        {
            try
            {
                var createdUser = await _userService.CreateUser(newUser);
                return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'CreateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpPut("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> UpdateUser(string userId, User user)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var userToUpdate = await _userService.GetUserById(userIdGuid);
                if (userToUpdate == null) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                await _userService.UpdateUser(userIdGuid, userToUpdate);
                return Ok(new SuccessResponse<User>() { Message = $"User updated with guid of {userToUpdate.UserId}", Data = userToUpdate });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'UpdateUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

        [HttpDelete("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
                var userToDelete = await _userService.GetUserById(userIdGuid);
                if (userToDelete == null || !await _userService.DeleteUser(userIdGuid)) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
                return Ok(new SuccessResponse<User>() { Message = $"User deleted with the guid of {userToDelete.UserId}", Data = userToDelete });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured while 'DeleteUser'");
                return StatusCode(500, new ErrorResponse() { Message = ex.Message });
            }
        }

    }
}