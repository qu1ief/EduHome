using EduHome.Models;

namespace EduHome.ViewModels;

public class BlogDetailVm
{
    public Blog Blog { get; set; } = null!;
    public List<BlogCategory> BlogCategories { get; set; } = new();
}
