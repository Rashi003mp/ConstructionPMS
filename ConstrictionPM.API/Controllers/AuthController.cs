using ConstructionPM.Application.Interfaces.Auth;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionPM.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserQueryRepository _userQuery;
    private readonly IJwtTokenGenerator _jwt;

    public AuthController(
        IUserQueryRepository userQuery,
        IJwtTokenGenerator jwt)
    {
        _userQuery = userQuery;
        _jwt = jwt;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(string email)
    {
        var user = await _userQuery.GetByEmailAsync(email);
        if (user == null)
            return Unauthorized("Invalid credentials");

        var token = _jwt.GenerateToken(user.Id, user.RoleName, user.Name);

        return Ok(new { token });
    }
}
