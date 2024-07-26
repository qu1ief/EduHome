using EduHome.Models.Common;

namespace EduHome.Models;

public class Event : BaseEntity
{
    public string Name { get; set; } = null!;
    public DateTime Date { get; set; }
    public string Time { get; set; }=null!;
    public string Venue { get; set; }=null!;
    public string Description { get; set; }=null!;
    public string ImagePath { get; set; }=null!;
    public ICollection<EventSpeaker> EventSpeakers { get; set; }=new List<EventSpeaker>();


}
