using Microsoft.EntityFrameworkCore;

namespace Product.Infrastructure
{
    public class ProductContext: DbContext
    {
        public DbSet<Domain.Models.Product> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ProductDb");
        }
    }
}
