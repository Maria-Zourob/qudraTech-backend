using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using QudraTech.Application.DTOs;
using QudraTech.Application.Interfaces;
using QudraTech.Domain.Entities;

namespace QudraTech.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullNameAr = request.FullNameAr,
            FullNameEn = request.FullNameEn
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        // أول مستخدم بالنظام يصير Super Admin تلقائيًا (للإعداد الأولي فقط)
        var isFirstUser = _userManager.Users.Count() == 1;
        var roleName = isFirstUser ? "SuperAdmin" : "InitiativeManager";

        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
        await _userManager.AddToRoleAsync(user, roleName);

        return Ok(new {message = "User registered", role = roleName});
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new {message = "Invalid email or password"});
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtTokenService.GenerateAccessToken(user, roles);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();

        return Ok(new AuthResponseDto(accessToken, refreshToken, user.Email!, roles));
    }
}