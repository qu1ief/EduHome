using EduHome.Models.Common;

namespace EduHome.Models;

public class EventSpeaker : BaseEntity
{
    public Event Event { get; set; } = null!;
    public int EventId { get; set; } 
    public int SpeakerId { get; set; } 
    public Speaker Speaker { get; set; } =null!;
}