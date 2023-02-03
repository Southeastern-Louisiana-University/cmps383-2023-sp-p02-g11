using Microsoft.AspNetCore.Identity;
using SP23.P02.Web.Features.UserRoles;

namespace SP23.P02.Web.Features.Users
{
    public class User : IdentityUser<int>
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string[] Roles { get; set; }

        public ICollection<UserRole> UserRoles { get; set; }
    }
}
