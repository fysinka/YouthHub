
using Microsoft.EntityFrameworkCore;
using YouthHub.Models;

namespace YouthHub.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Initiative> Initiatives { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Application> Applications { get; set; }
}
