using EduHome.Models;

namespace EduHome.ViewModels;

public class HomeVm
{
    public List<Course> Courses { get; set; } = new();
    public List<Event> Events{ get; set; } = new();
    public List<Blog> Blogs{ get; set; } = new();
}
