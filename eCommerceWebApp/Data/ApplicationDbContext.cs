using eCommerceWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace eCommerceWebApp.Data
{
    public class ApplicationDbContext:DbContext
    {
        private readonly ApplicationDbContext _db;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {


        }

        public DbSet<Category> categories { get; set; }

        public DbSet<Product> products { get; set; }
    }
}
