using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace  Bulky.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)  
        {
            
        }



        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //this repeated for each class
           // modelBuilder.ApplyConfiguration(new CategorySeed());

            //this done for all 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
