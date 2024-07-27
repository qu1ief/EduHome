using EduHome.Models;

namespace EduHome.ViewModels;

public class CourseDetailVm
{
    public Course Course { get; set; } = null!;
    public List<Category> Categories { get; set; } = new();
}
