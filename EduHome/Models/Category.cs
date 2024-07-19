using EduHome.Models.Common;

namespace EduHome.Models;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();


}
