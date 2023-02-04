using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP23.P02.Web.Features.Authorization;
using SP23.P02.Web.Features.Users;
namespace SP23.P01.Web.Controllers;


[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly SignInManager<IdentityUser> signInManager;


    public AuthenticationController
        (
        UserManager<IdentityUser> userManager, 
        SignInManager<IdentityUser> signInManager
        )
    {
        this.userManager = userManager;
        this.signInManager = signInManager;

    }


  
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<UserDto>> Login( LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.UserName);

        if (user == null)
        {
            return BadRequest();
        }
        if(user.UserName== null) { 
            return BadRequest();
        }

        var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password,false);

        if (!result.Succeeded)
        {
            return BadRequest();
        }
        await signInManager.SignInAsync(user,false);

        var dtoReturn = await GetUDto((IQueryable<User>)userManager.Users).SingleAsync(x => x.UserName == user.UserName);
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
       
        var resultDto = await GetUDto((IQueryable<User>)userManager.Users).SingleAsync(x => x.UserName == user);
        return Ok(resultDto);
    }

    private static IQueryable<UserDto> GetUDto(IQueryable<User> users)
    {
        return users.Select(x => new UserDto
        {
            Id = x.Id,
            UserName = x.UserName,
            Roles = x.Roles.Select(y => y.Role!.Name).ToArray()
        });
    }
}

