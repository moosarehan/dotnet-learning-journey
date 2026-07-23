using Microsoft.EntityFrameworkCore;
using myfirstrestapi.Entities;

namespace myfirstrestapi.Data
{
    public class AppDBcontext:DbContext
    {
        public AppDBcontext(DbContextOptions<AppDBcontext> options):base(options)
        {

                
        }


        public DbSet<User> AccountUser { get; set; }
        public DbSet<Employe> Employees { get; set; }

    }
}
