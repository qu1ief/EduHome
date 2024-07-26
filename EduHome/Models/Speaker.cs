using EduHome.Models.Common;

namespace EduHome.Models;

public class Speaker:BaseEntity
{
    public string Fullname { get; set; } = null!;
    public string Profession { get; set; } = null!;
    public string ImagePath { get; set; } = null!;
    public ICollection<EventSpeaker> EventSpeakers { get; set; } = new List<EventSpeaker>();

}
