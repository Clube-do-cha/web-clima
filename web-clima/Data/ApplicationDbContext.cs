using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using web_clima.Models;

namespace web_clima.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Adicione DbSets para outras entidades, se houver.
        // Por exemplo:
        // public DbSet<OutraEntidade> OutraEntidades { get; set; }
    }
}
