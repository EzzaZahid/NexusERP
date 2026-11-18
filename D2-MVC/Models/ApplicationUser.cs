
using Microsoft.AspNetCore.Identity;

namespace NexusERP_D2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public string Department { get; set; }
    }
}
