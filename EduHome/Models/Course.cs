using EduHome.Models.Common;

namespace EduHome.Models;

public class Course : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int ClassDuration { get; set; }
    public string SkillLevel { get; set; } = null!;
    public string Language { get; set; } = null!;
    public int StudentCount { get; set; }
    public string Assesments { get; set; } = null!;
    public decimal Price { get; set; }

    public ICollection<CourseCategory> CourseCategories { get; set; } = new List<CourseCategory>();

}
