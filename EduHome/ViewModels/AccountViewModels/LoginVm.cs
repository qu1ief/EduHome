using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class LoginVm
{
    [Required]
    public string UsernameOrEmail { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; }=null!;
    public bool RememberMe { get; set; }
}

