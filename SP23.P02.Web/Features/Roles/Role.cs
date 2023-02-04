using Microsoft.AspNetCore.Identity;
using SP23.P02.Web.Features.Authorization;

namespace SP23.P02.Web.Features.Roles
{
    public class Role : IdentityRole<int>
    {
        public const string Admin = nameof(Admin);

        public const string User = nameof(User);

        public ICollection<UserRole> Users { get; set; } = new List<UserRole>();
    }
}