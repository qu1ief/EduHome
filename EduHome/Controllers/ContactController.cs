using EduHome.Contexts;
using EduHome.Services;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class ContactController : Controller
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;

    public ContactController(AppDbContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public async Task<IActionResult> Index()
    {
        var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

        return View(settings);
    }

    [HttpPost]
    public IActionResult Index(ContactVm vm)
    {
        if (!ModelState.IsValid)
            return Content("");



        string htmlCode = @$"
<!DOCTYPE html>
<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>User Contact Info</title>
    <style>
        body {{
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
        }}
        .container {{
            max-width: 600px;
            margin: 50px auto;
            background-color: #33691e;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        h2 {{
            color: #333;
        }}
        .info {{
            margin-bottom: 20px;
        }}
        .info label {{
            font-weight: bold;
        }}
        .info p {{
            margin: 5px 0;
        }}
        .message {{
            margin-bottom: 20px;
        }}
        .message label {{
            font-weight: bold;
        }}
        .message p {{
            margin: 5px 0;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h2>User Contact Information</h2>
        <div class=""info"">
            <label for=""name"">Name:</label>
            <p>{vm.Name}</p>
            <label for=""subject"">Subject:</label>
            <p>{vm.Subject}</p>
            <label for=""email"">Email:</label>
            <p>{vm.Email}</p>
        </div>
        <div class=""message"">
            <label for=""message"">Message:</label>
            <p>{vm.Message}</p>
        </div>
    </div>
</body>
</html>";


        _emailService.SendEmail(new(body: htmlCode, subject: "Contact Info", to: "sabirkg@code.edu.az"));



        return Content("");

    }
}
