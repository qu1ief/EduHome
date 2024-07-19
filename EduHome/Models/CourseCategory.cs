using EduHome.Models.Common;

namespace EduHome.Models;

public class CourseCategory : BaseEntity
{
    public int CategoryId { get; set; }
    public Category Category { get; set; }= null!;
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
}