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


        public DbSet<JwtToken> JwtTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<JwtToken>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Token).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.ExpiryDate).IsRequired();
            });
        }



    }
}
