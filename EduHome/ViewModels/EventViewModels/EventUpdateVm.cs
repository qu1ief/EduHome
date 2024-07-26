using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class EventUpdateVm
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Time { get; set; } = null!;
    public string Venue { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IFormFile? Image { get; set; }
    public string? ImagePath { get; set; }
    [Required]
    public List<int> SpeakerIds { get; set; } = new List<int>();
}

