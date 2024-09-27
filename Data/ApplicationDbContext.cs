using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Models;
using Microsoft.AspNetCore.Identity;

namespace UserManagementApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // If you want to keep the Users property, you can override it
        public override DbSet<ApplicationUser> Users => base.Users;

        // If you want to define a new property, you can do it like this
        // public new DbSet<ApplicationUser> Users { get; set; }

        // Other DbSets can be defined here
    }
}
