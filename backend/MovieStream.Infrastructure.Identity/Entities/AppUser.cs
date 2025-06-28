using Microsoft.AspNetCore.Identity;

namespace MovieStream.Infrastructure.Identity.Entities
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
