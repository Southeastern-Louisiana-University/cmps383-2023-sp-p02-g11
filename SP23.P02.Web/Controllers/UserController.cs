using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SP23.P02.Web.Features.Roles;
using SP23.P02.Web.Features.Authorization;
using SP23.P02.Web.Features.Users;


namespace SP23.P02.Web.Controllers
{
    [ApiController]
    [Route("api/users")]

    public class UserController : ControllerBase
    {

        public class UsersController : ControllerBase
        {
            private readonly UserManager<User> userManager;
            
            public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
            {
                this.userManager = userManager;
                
            }


            [HttpPost("new")]
            [Authorize(Roles = Role.Admin)]
            public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
            {
                
                var newUser = new User
                {
                    UserName = dto.UserName,
                };
                var createResult = await userManager.CreateAsync(newUser, dto.Password);
                if (!createResult.Succeeded)
                {
                    return BadRequest();
                }


                var roleResult = await userManager.AddToRolesAsync(newUser, dto.Roles);

                if (!roleResult.Succeeded)
                {
                    return BadRequest();
                }
               
                return Ok(new UserDto
                {
                    Id = newUser.Id,
                    Roles = dto.Roles,
                    UserName = newUser.UserName,
                });


            }
        }
    }
}



