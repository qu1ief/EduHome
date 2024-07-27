using EduHome.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Contexts;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }


    public DbSet<Course> Courses { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Blog> Blogs { get; set; } = null!;
    public DbSet<BlogCategory> BlogCategories { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Speaker> Speakers { get; set; } = null!;
    public DbSet<Teacher> Teachers { get; set; } = null!;
    public DbSet<Skill> Skills { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<Subscribe> Subscribes  { get; set; } = null!;



}
