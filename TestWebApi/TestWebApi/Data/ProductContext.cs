using Microsoft.EntityFrameworkCore;
using TestWebApi.Models;

namespace TestWebApi.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } = null!;
    }
}
