using EduHome.Contexts;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
public class CourseController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _imagePath;

    public CourseController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
    }

    public async Task<IActionResult> Index()
    {
        var courses = await _context.Courses.ToListAsync();

        return View(courses);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CourseCreateVm vm)
    {
        if ((!ModelState.IsValid))
            return View(vm);


        if (!vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please enter valid image");
            return View(vm);
        }

        foreach (var categoryId in vm.CategoryIds)
        {
            var isExistCategory = await _context.Categories.AnyAsync(x => x.Id == categoryId);

            if (!isExistCategory)
            {
                ModelState.AddModelError("CategoryIds", "This category is not found");
                return View(vm);
            }
        }

        var isExist = await _context.Courses.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower().Trim());

        if (isExist)
        {
            ModelState.AddModelError("Name", "This name is already exist");
            return View(vm);
        }


        string filename = await vm.Image.CreateFileAsync(_imagePath);

        Course course = new()
        {
            Name = vm.Name,
            Assesments = vm.Assesments,
            ClassDuration = vm.ClassDuration,
            Description = vm.Description,
            Duration = vm.Duration,
            ImagePath = filename,
            Language = vm.Language,
            Price = vm.Price,
            SkillLevel = vm.SkillLevel,
            StartTime = vm.StartTime,
            StudentCount = vm.StudentCount,
            CourseCategories = new List<CourseCategory>()
        };


        foreach (var categoryId in vm.CategoryIds)
        {
            CourseCategory courseCategory = new()
            {
                CategoryId = categoryId,
                Course = course
            };

            course.CourseCategories.Add(courseCategory);
        }


        await _context.Courses.AddAsync(course);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");


    }

}
