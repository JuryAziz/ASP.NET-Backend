#pragma warning disable CS8602 // Dereference of a possibly null reference.
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Store.Dtos;
using Store.EntityFramework;
using Store.EntityFramework.Entities;
using Store.Helpers.Enums;

namespace Store.Application.Services;

public class AuthSerivce(AppDbContext appDbContext, IMapper mapper, IConfiguration? configuration = default, IPasswordHasher<User>? passwordHasher = default) 
{
    private readonly IConfiguration? _configuration = configuration;
    private readonly IPasswordHasher<User>? _passwordHasher = passwordHasher;
    private readonly IMapper _mapper = mapper;
    private readonly AppDbContext _appDbContext = appDbContext;

    #pragma warning disable CS8604 // Possible null reference argument.
    #pragma warning disable CS8629 // Nullable value type may be null.
    public string? GenerateJwtToken(UserDto userDto) {
        JwtSecurityTokenHandler tokenHandler = new ();
        byte[] key = Encoding.ASCII.GetBytes(_configuration?["Jwt:Key"]);


        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.NameIdentifier, userDto.UserId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, Enum.GetName((UserRole)userDto.Role) ),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration?["Jwt:Audience"]),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration?["Jwt:Issuer"])
            ]),

            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<object?> Login(LoginDto loginDto) 
    {
        User? foundUser = _appDbContext.Users.FirstOrDefault(user => user.Email.ToLower() == loginDto.Email.ToLower()); 
        if(foundUser is null) return null;

        PasswordVerificationResult passwordVerified = _passwordHasher.VerifyHashedPassword(foundUser, foundUser.Password, loginDto.Password);
        if (passwordVerified is not PasswordVerificationResult.Success) return null;

        UserDto userDto = _mapper.Map<UserDto>(foundUser);

        var token = GenerateJwtToken(userDto);

        return await Task.FromResult(new { userDto, token }); 
    } 

    public async Task<object?> ResetPassword(ResetPasswordDto resetPasswordDto) 
    {
        User? foundUser = _appDbContext.Users.FirstOrDefault(user => user.Email.ToLower() == resetPasswordDto.Email.ToLower()); 
        if(foundUser is null) return null;

        #pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
        foundUser.Password = _passwordHasher.HashPassword(null, resetPasswordDto.NewPassword);

        await _appDbContext.SaveChangesAsync();

        return await Task.FromResult(resetPasswordDto); 
    } 

    public string? Authenticate(string userId, UserRole roleReq) 
    {
        User? foundUser = _appDbContext.Users.FirstOrDefault(user => user.UserId.ToString() == userId);
        if(foundUser is null) return "Unknown user";
        if((UserRole)foundUser.Role == UserRole.Banned) return "Banned user";
        if((UserRole)foundUser.Role != roleReq) return "Unauthorized user";
        return null;
    }
}