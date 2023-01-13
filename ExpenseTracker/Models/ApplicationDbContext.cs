using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Models
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Categories> Categories { get; set; }

    }
}
