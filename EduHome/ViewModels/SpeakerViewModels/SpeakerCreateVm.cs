namespace EduHome.ViewModels;

public class SpeakerCreateVm
{
    public string Fullname { get; set; } = null!;
    public string Profession { get; set; } = null!;
    public IFormFile Image { get; set; } = null!;
}
