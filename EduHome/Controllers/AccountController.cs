using EduHome.Models;
using EduHome.Services;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IEmailService _emailService;

    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _emailService = emailService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);

        if (user is null)
            user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);

        if (user is null)
        {
            ModelState.AddModelError("", "Invalid credential");
            return View(vm);
        }


        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "User is blocked,please try 5 minute ago");
                return View(vm);
            }

            if (!user.EmailConfirmed)
            {
                ModelState.AddModelError("", "Please confirm your email adress");
                return View(vm);
            }

            ModelState.AddModelError("", "Invalid credential");
            return View(vm);
        }


        return RedirectToAction("Index", "Home");
    }


    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        AppUser user = new()
        {
            Email = vm.Email,
            Fullname = vm.Fullname,
            UserName = vm.Username,
        };


        var result = await _userManager.CreateAsync(user, vm.Password);


        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(vm);
        }

        await _userManager.AddToRoleAsync(user, "User");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var link = Url.Action("VerifyEmail", "Account", new { email = user.Email, token = token }, HttpContext.Request.Scheme);


        string emailBody = @$"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Confirm Your Email Address</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #007bff; /* Blue base color */
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 50px auto;
            background-color: #ffffff; /* White base color */
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        .message {{
            color: #333;
            margin-bottom: 20px;
        }}
        .confirmation-link {{
            display: inline-block;
            padding: 10px 20px;
            background-color: #0056b3; /* Darker blue for link */
            color: #fff;
            text-decoration: none;
            border-radius: 5px;
        }}
        .confirmation-link:hover {{
            background-color: #003d7a; /* Even darker blue on hover */
        }}

        a{{
            font-style: none;

        }}
    </style>
</head>
<body>
    <div class=""container"">
        <p class=""message"">Dear {user.UserName},</p>
        <p class=""message"">Please click the following link to confirm your email address and complete your registration:</p>
        <a href=""{link}"" class=""confirmation-link"">Confirm Email</a>
        <p class=""message"">If you did not request this, please ignore this email.</p>
        <p class=""message"">Regards,<br>EduHome Team</p>
    </div>
</body>
</html>";


        _emailService.SendEmail(new(body: emailBody, subject: "Email Verification", to: vm.Email));



        return RedirectToAction("ConfirmEmail");
    }


    public IActionResult ConfirmEmail()
    {
        return View();
    }


    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Login");
    }


    public async Task<IActionResult> VerifyEmail(string? email, string? token)
    {
        if (token == null || email == null)
            return BadRequest();

        var user = await _userManager.Users.FirstOrDefaultAsync(e => e.Email == email);

        if (user is null)
            return NotFound();

        var verify = await _userManager.ConfirmEmailAsync(user, token);
        if (verify.Succeeded)
        {
            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        return BadRequest();
    }

    public async Task<IActionResult> CreateRoles()
    {
        IdentityRole role1 = new() { Name = "Admin" };
        IdentityRole role2 = new() { Name = "User" };

        await _roleManager.CreateAsync(role1);
        await _roleManager.CreateAsync(role2);

        return Content("Ok");
    }
}
