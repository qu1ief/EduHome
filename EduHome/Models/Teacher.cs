using EduHome.Models.Common;

namespace EduHome.Models;

public class Teacher : BaseEntity
{
    public string Fullname { get; set; } = null!;
    public string Profession { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Degree { get; set; } = null!;
    public string Experience { get; set; } = null!;
    public string Hobbies { get; set; } = null!;
    public string Faculty { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string Skype { get; set; } = null!;
    public string FacebookLink { get; set; } = null!;
    public string PinterestLink { get; set; } = null!;
    public string VKLink { get; set; } = null!;
    public string TwitterLink { get; set; } = null!;
    public string ImagePath { get; set; } = null!;

    public ICollection<Skill> Skills { get; set; } = new List<Skill>();

}
