using EduHome.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace EduHome.Models;

public class Skill : BaseEntity
{
    public string Name { get; set; } = null!;
    [Range(0, 100)]
    public int Point { get; set; }
    public Teacher Teacher { get; set; } = null!;
    public int TeacherId { get; set; }
}
