using Microsoft.AspNetCore.Identity;

namespace EduHome.Models;

public class AppUser:IdentityUser
{
    public string Fullname { get; set; } = null!;
}
