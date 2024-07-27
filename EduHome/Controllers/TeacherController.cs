using EduHome.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class TeacherController:Controller
{
    private readonly AppDbContext _context;

    public TeacherController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var teachers=await _context.Teachers.ToListAsync();

        return View(teachers);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var teacher=await _context.Teachers.Include(x=>x.Skills).FirstOrDefaultAsync(x=>x.Id==id);

        if(teacher is null)
            return NotFound();  


        return View(teacher);
    }
}
