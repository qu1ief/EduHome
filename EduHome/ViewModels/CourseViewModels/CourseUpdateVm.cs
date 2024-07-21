namespace EduHome.ViewModels;

public class CourseUpdateVm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile? Image { get; set; } 
    public string? ImagePath{ get; set; } 
    public DateTime StartTime { get; set; }
    public int Duration { get; set; }
    public int ClassDuration { get; set; }
    public string SkillLevel { get; set; } = null!;
    public string Language { get; set; } = null!;
    public int StudentCount { get; set; }
    public string Assesments { get; set; } = null!;
    public decimal Price { get; set; }

    public List<int> CategoryIds { get; set; }
}
