using Microsoft.AspNetCore.Identity;

namespace Tabemory.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int UserId { get; set; }

        public virtual ICollection<Record> Records { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
