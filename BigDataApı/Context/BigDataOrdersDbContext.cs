using BigDataApı.Entities;
using Microsoft.EntityFrameworkCore;

namespace BigDataApı.Context
{
    public class BigDataOrdersDbContext:DbContext
    {
        public BigDataOrdersDbContext(DbContextOptions<BigDataOrdersDbContext> options) : base(options)
        {
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
    }
}
