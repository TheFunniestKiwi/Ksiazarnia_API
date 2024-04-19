using Microsoft.AspNetCore.Identity;

namespace Ksiazarnia_API.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string LastName { get; set; }
    }
}
