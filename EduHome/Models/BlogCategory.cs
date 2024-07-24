using EduHome.Models.Common;

namespace EduHome.Models;

public class BlogCategory : BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
