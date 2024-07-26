using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class TeacherUpdateVm
{
    public int Id { get; set; }
    public string Fullname { get; set; } = null!;
    public string Profession { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string Experience { get; set; } = null!;
    public string Hobbies { get; set; } = null!;
    public string Faculty { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Phone]
    public string PhoneNumber { get; set; } = null!;
    public string Skype { get; set; } = null!;
    public IFormFile? Image { get; set; } 
    public string? ImagePath { get; set; } 


}
