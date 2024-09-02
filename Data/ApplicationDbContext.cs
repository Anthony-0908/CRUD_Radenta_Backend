using CRUD_Radenta.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUD_Radenta.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options) // This line is crucial for passing options to the base class
        {
        }

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Product> Products { get; set; }



    }
}
