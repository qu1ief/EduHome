namespace EduHome.ViewModels;

public class SpeakerUpdateVm
{
    public int Id { get; set; }
    public string Fullname { get; set; } = null!;
    public string Profession { get; set; } = null!;
    public IFormFile? Image { get; set; } 
    public string? ImagePath { get; set; } 
}
