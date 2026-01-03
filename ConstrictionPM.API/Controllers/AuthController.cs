using ConstructionPM.Application.DTOs;
using ConstructionPM.Application.Interfaces.Auth;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionPM.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserQueryRepository _userQuery;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AuthController(
        IUserQueryRepository userQuery,
        IJwtTokenGenerator jwt,
        IPasswordHasher<User> passwordHasher)
    {
        _userQuery = userQuery;
        _jwt = jwt;
        _passwordHasher = passwordHasher;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var user = await _userQuery.GetForLoginAsync(request.Email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        // Password verification
        var result = _passwordHasher.VerifyHashedPassword(
            user: null!,                  // not needed by implementation
            hashedPassword: user.PasswordHash,
            providedPassword: request.Password
        );

        if (result == PasswordVerificationResult.Failed)
            return Unauthorized("Invalid credentials");

        var token = _jwt.GenerateToken(
            user.Id,
            user.RoleName,
            user.Name
        );

        return Ok(new { token });
    }
}
