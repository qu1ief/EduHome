using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class ContactVm
{
    public string Name { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Message { get; set; } = null!;
    public string returnUrl { get; set; } = null!;

}
