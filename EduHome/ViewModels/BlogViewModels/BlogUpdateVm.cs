namespace EduHome.ViewModels;

public class BlogUpdateVm
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Author { get; set; } = null!;
    public IFormFile? Image { get; set; } = null!;
    public string? ImagePath { get; set; } = null!;
    public int BlogCategoryId { get; set; }
}

