using EduHome.Contexts;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class AboutController:Controller
{
    private readonly AppDbContext _context;

    public AboutController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers.Take(4).ToListAsync();

        AboutVm vm = new()
        {
            Teachers = teachers
        };

        return View(vm);
    }
}
