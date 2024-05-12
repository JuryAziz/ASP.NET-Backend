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
[Route("/api/auth")]
public class AuthController(AppDbContext appDbContext, IPasswordHasher<User> passwordHasher, IMapper mapper, IConfiguration configuration) : ControllerBase
{
    private readonly AuthSerivce _authService = new (appDbContext, mapper, configuration, passwordHasher);

    [HttpPost("login")]
    public async Task<IActionResult> UserLogin([FromBody] LoginDto loginDto)
    {
        var userLoggedIn = await _authService.Login(loginDto);
        if (userLoggedIn is null) return Unauthorized(new BaseResponse<UserDto>(false, "Unable to verify user cerdentials!"));

        return Ok(new BaseResponse<object>(userLoggedIn, true));
    }
}