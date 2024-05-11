using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "KALEM 1", Price = 300 },
                    new Product() { Id = 2, Name = "KALEM 2", Price = 300 },
                     new Product() { Id = 3, Name = "KALEM 3", Price = 300 });



            base.OnModelCreating(modelBuilder);
        }
    }
}
