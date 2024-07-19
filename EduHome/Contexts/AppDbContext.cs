using EduHome.Models;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
        
    }


    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Category> Categories{ get; set; } = null!;
}
