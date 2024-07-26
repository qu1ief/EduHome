using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class TeacherSkillVm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    [Range(0, 100)]
    public int Point { get; set; }
}