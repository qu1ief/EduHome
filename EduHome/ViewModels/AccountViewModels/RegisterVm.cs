using System.ComponentModel.DataAnnotations;

namespace EduHome.ViewModels;

public class RegisterVm
{

    [Required, MaxLength(100)]
    public string Fullname { get; set; } = null!;
    [Required, MaxLength(30)]
    public string Username { get; set; } = null!;
    [Required, MaxLength(256), DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = null!;
    [Required, DataType(DataType.Password), Compare(nameof(Password))]
    public string ConfirmPassword { get; set; } = null!;
}
