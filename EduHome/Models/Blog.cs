using EduHome.Models.Common;

namespace EduHome.Models;

public class Blog:BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set;} = null!;
    public string Author { get; set;} = null!;
    public string ImagePath { get; set;} = null!;
    public DateTime CreatedTime { get; set;}
    public BlogCategory BlogCategory { get; set; } = null!;
    public int BlogCategoryId { get; set; } 

}
