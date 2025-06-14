using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Contracts;

namespace Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController(IUserAccount accountInterface) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> CreateAsync(Register user)
    {
        if (user is null) return BadRequest("Model is empty");
        var result = await accountInterface.CreateAsync(user);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> SignInAsync(Login user)
    {
        if (user is null) return BadRequest("Model is empty");
        var result = await accountInterface.SignInAsync(user);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshTokenAsync(RefreshToken token)
    {
        if (token is null) return BadRequest("Model is empty");
        var result = await accountInterface.RefreshTokenAsync(token);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersAsync()
    {
        var users = await accountInterface.GetUsers();
        if (users is null) return NotFound();
        return Ok(users);
    }

    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser(ManagerUser managerUser)
    {
        var result = await accountInterface.UpdateUser(managerUser);
        return Ok(result);
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        var roles = await accountInterface.GetRoles();
        if (roles is null) return NotFound();
        return Ok(roles);
    }

    [HttpDelete("delete-user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await accountInterface.DeleteUser(id);
        return Ok(result);
    }
}

