using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.Helpers;
using Store.Models;

namespace Store.API.Controllers;
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
            IEnumerable<UserModel> users = await _userService.GetUsers(page, limit);
            return Ok(new BaseResponseList<UserModel>(users, true));
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
            UserModel? foundUser = await _userService.GetUserById(userIdGuid);
            if (foundUser == null) return NotFound();
            return Ok(new BaseResponse<UserModel>(foundUser, true));

        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'GetUserById'");
            return StatusCode(500, ex.Message );
        }
    }

    // [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}/addresses")]
    // public async Task<IActionResult> GetUserAddresses(string userId)
    // {
    //     try
    //     {
    //         if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
    //         var foundUserAddress = await _addressService.GetUserAddresses(userIdGuid);
    //         if (foundUserAddress == null) return StatusCode(404, new SuccessResponse<string>() { Data = "Not Found" });
    //         return Ok(new SuccessResponse<IEnumerable<UserModel>>() { Message = $"User address found belonding to Guid {userIdGuid}", Data = foundUserAddress });
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"An error occured while 'GetUserAddresses'");
    //         return StatusCode(500, new ErrorResponse() { Message = ex.Message });
    //     }
    // }

    // [HttpGet("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}/paymentmethods")]
    // public async Task<IActionResult> GetUserPaymentMethods(string userId)
    // {
    //     try
    //     {
    //         if (!Guid.TryParse(userId, out Guid userIdGuid)) return StatusCode(500, new ErrorResponse() { Message = "Invalid Guid Format" });
    //         var foundUserPaymentMethods = await _paymentMethodService.GetUserPaymentMethods(userIdGuid);
    //         if (foundUserPaymentMethods == null) return StatusCode(404, new SuccessResponse<string>() { Message = "Not Found" });
    //         return Ok(new SuccessResponse<IEnumerable<UserModel>>() { Message = $"User payment methods found belonding to Guid {userIdGuid}", Data = foundUserPaymentMethods });
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine($"An error occured while 'GetUserPaymentMethods'");
    //         return StatusCode(500, new ErrorResponse() { Message = ex.Message });
    //     }
    // }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserModel newUser)
    {
        try
        {
            UserModel? createdUser = await _userService.CreateUser(newUser);
            return CreatedAtAction(nameof(GetUserById), new { createdUser?.UserId }, createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'CreateUser'");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut("{userId:regex(^[[0-9a-f]]{{8}}-[[0-9a-f]]{{4}}-[[0-5]][[0-9a-f]]{{3}}-[[089ab]][[0-9a-f]]{{3}}-[[0-9a-f]]{{12}}$)}")]
    public async Task<IActionResult> UpdateUser(string userId, UserModel user)
    {
        try
        {
            if (!Guid.TryParse(userId, out Guid userIdGuid)) return BadRequest("Invalid User ID Format");
            var userToUpdate = await _userService.GetUserById(userIdGuid);
            if (userToUpdate == null) return BadRequest(ModelState);
            UserModel? updatedUser = await _userService.UpdateUser(userIdGuid, userToUpdate);
            return Ok(new BaseResponse<UserModel>(updatedUser, true));
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
            UserModel? userToDelete = await _userService.GetUserById(userIdGuid);
            if (userToDelete == null || !await _userService.DeleteUser(userIdGuid)) return NotFound();
            return Ok(new BaseResponse<UserModel>(userToDelete, true));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occured while 'DeleteUser'");
            return StatusCode(500, ex.Message);
        }
    }

}