using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Data;
using SP23.P02.Web.Features.Authorization;
using SP23.P02.Web.Features.Users;
using System.Linq.Expressions;

namespace SP23.P01.Web.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> userManager;
    private readonly SignInManager<User> signInManager;

    public AuthenticationController
        (
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.UserName);

        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (string.IsNullOrEmpty(user.UserName))
        {
            return BadRequest("Username cannot be empty.");
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

        if (!result.Succeeded)
        {
            return BadRequest();
        }
        await signInManager.SignInAsync(user, true);
        /*
        var resultant = new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Roles = user.Roles.Select(x => x.Role.Name).ToArray()
            };
        
        return Ok(resultant);
        */
        var dtoReturn = await GetUDto(userManager.Users).SingleAsync(x => x.UserName == user.UserName);
        return Ok(dtoReturn);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return Ok();
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var user = User.GetCurrentUserName();
        if (user == null)
        {
            return Unauthorized();
        }

        var resultDto = await GetUDto(userManager.Users).SingleAsync(x => x.UserName == user);
        return Ok(resultDto);
    }

    private static IQueryable<UserDto> GetUDto(IQueryable<User> users)
    {
        return users.Select(x => new UserDto
        {
            Id = x.Id,
            UserName = x.UserName,
            Roles = x.Roles.Select(y => y.Role.Name).ToArray()
        }) ;
    }
}

