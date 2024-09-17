using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace JerseyShop.ViewModels
{
    public class UserWithRolesViewModel
    {
        public IdentityUser User { get; set; }
        public List<string> Roles { get; set; }
    }
}
