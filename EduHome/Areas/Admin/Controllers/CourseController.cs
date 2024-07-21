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

        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;


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


    public async Task<IActionResult> Delete(int id)
    {
        var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);

        if (course is null)
            return NotFound();


        _context.Courses.Remove(course);
        await _context.SaveChangesAsync();

        course.ImagePath.DeleteFile(_imagePath);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var course = await _context.Courses.Include(x => x.CourseCategories).FirstOrDefaultAsync(x => x.Id == id);

        if (course is null)
            return NotFound();

        CourseUpdateVm vm = new()
        {
            Id = id,
            Name = course.Name,
            Assesments = course.Assesments,
            CategoryIds = course.CourseCategories.Select(x => x.CategoryId).ToList(),
            ClassDuration = course.ClassDuration,
            Description = course.Description,
            Duration = course.Duration,
            ImagePath = course.ImagePath,
            Language = course.Language,
            Price = course.Price,
            SkillLevel = course.SkillLevel,
            StartTime = course.StartTime,
            StudentCount = course.StudentCount,

        };



        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(CourseUpdateVm vm)
    {

        var categories = await _context.Categories.ToListAsync();
        ViewBag.Categories = categories;

        if (!ModelState.IsValid)
            return View(vm);

        var existCourse = await _context.Courses.Include(x => x.CourseCategories).FirstOrDefaultAsync(x => x.Id == vm.Id);


        if (existCourse is null)
            return BadRequest();


        if (vm.Image is not null && !vm.Image.ValidateImage())
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




        var isExist = await _context.Courses.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower().Trim() && x.Id != vm.Id);

        if (isExist)
        {
            ModelState.AddModelError("Name", "This name is already exist");
            return View(vm);
        }




        existCourse.Name = vm.Name;
        existCourse.StudentCount = vm.StudentCount;
        existCourse.Duration = vm.Duration;
        existCourse.Description = vm.Description;
        existCourse.Assesments = vm.Assesments;
        existCourse.ClassDuration = vm.ClassDuration;
        existCourse.Language = vm.Language;
        existCourse.Price = vm.Price;
        existCourse.SkillLevel = vm.SkillLevel;
        existCourse.StartTime = vm.StartTime;
        existCourse.CourseCategories = new List<CourseCategory>();



        foreach (var categoryId in vm.CategoryIds)
        {
            CourseCategory courseCategory = new()
            {
                CategoryId = categoryId,
                Course = existCourse,
                CourseId = existCourse.Id
            };

            existCourse.CourseCategories.Add(courseCategory);
        }


        if (vm.Image is not null)
        {
            existCourse.ImagePath.DeleteFile(_imagePath);
            existCourse.ImagePath = await vm.Image.CreateFileAsync(_imagePath);
        }


        _context.Courses.Update(existCourse);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Detail(int id)
    {
        var course = await _context.Courses.Include(x=>x.CourseCategories).ThenInclude(x=>x.Category).FirstOrDefaultAsync(x => x.Id == id);

        if (course is null)
            return NotFound();


        return View(course);
    }

}
