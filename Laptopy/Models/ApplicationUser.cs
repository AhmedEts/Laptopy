using Microsoft.AspNetCore.Identity;

namespace Laptopy.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string? Address { get; set; }
    }
}
