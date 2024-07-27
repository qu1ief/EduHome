using EduHome.Contexts;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class CourseController:Controller
{
    private readonly AppDbContext _context;

    public CourseController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string? search,int? categoryId)
    {
        var query =  _context.Courses.AsQueryable();

        if(search is not null)
            query=query.Where(x=>x.Name.Contains(search) || x.Description.Contains(search));

        if(categoryId is not null)
            query=query.Where(x=>x.CourseCategories.Any(c=>c.CategoryId == categoryId));

        var courses = await query.ToListAsync();

        return View(courses);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var course=await _context.Courses.FirstOrDefaultAsync(x=>x.Id==id);

        if(course is null)
            return NotFound();

        var categories=await _context.Categories.Include(x=>x.CourseCategories).ToListAsync();

        CourseDetailVm vm = new()
        {
            Course = course,
            Categories = categories
        };

        return View(vm);
    }
}
