using Microsoft.AspNetCore.Mvc;

using Store.Application.Services;
using Store.EntityFramework.Entities;
using Store.EntityFramework;
using Store.Helpers;
using Store.Dtos;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Store.API.Controllers;
[ApiController]
[Route("/api/auth")]
public class AuthController(AppDbContext appDbContext, IPasswordHasher<User> passwordHasher, IMapper mapper, IConfiguration configuration) : ControllerBase
{
    private readonly AuthSerivce _authService = new (appDbContext, mapper, configuration, passwordHasher);
    private readonly UserService _userService = new (appDbContext, mapper, passwordHasher);

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto newUser)
    {
        UserDto? createdUser = await _userService.CreateUser(newUser);
        return Ok(new BaseResponse<UserDto>(createdUser, true));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var userLoggedIn = await _authService.Login(loginDto);
        if (userLoggedIn is null) return Unauthorized(new BaseResponse<UserDto>(false, "Unable to verify user cerdentials!"));

        return Ok(new BaseResponse<object>(userLoggedIn, true));
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        var userLoggedIn = await _authService.ResetPassword(resetPasswordDto);
        if (userLoggedIn is null) return BadRequest(new BaseResponse<UserDto>(false, "Something went wrong!"));

        return Ok(new BaseResponse<object>(userLoggedIn, true, "Password changed successfully!"));
    }
}