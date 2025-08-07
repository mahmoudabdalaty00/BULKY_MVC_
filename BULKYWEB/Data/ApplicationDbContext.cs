using BULKYWEB.Data.Configuration;
using BULKYWEB.Models;
using Microsoft.EntityFrameworkCore;

namespace BULKYWEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)  
        {
            
        }



        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //this repeated for each class
           // modelBuilder.ApplyConfiguration(new CategorySeed());

            //this done for all 
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
