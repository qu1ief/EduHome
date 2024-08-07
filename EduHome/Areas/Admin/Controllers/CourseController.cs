﻿using EduHome.Contexts;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.Services;
using EduHome.ViewModels;
using EduHome.ViewModels.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CourseController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IEmailService _emailService;
    private string _imagePath;

    public CourseController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IEmailService emailService)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
        _emailService = emailService;
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


        //send subscribes



        var subscribes = await _context.Subscribes.ToListAsync();

        string url = Url.Action("Detail", "Course", new { area = "", id = course.Id }, HttpContext.Request.Scheme) ?? "";

        string emailBody = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #ffffff;
            color: #000000;
            margin: 0;
            padding: 0;
        }}
        .container {{
            width: 100%;
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
        }}
        .header {{
            background-color: #cc9933;
            padding: 20px;
            text-align: center;
            color: #ffffff;
        }}
        .content {{
            padding: 20px;
            background-color: #f9f9f9;
            color: #000000;
        }}
        .footer {{
            background-color: #cc9933;
            padding: 10px;
            text-align: center;
            color: #ffffff;
        }}
        .button {{
            display: inline-block;
            padding: 10px 20px;
            margin: 20px 0;
            color: #ffffff;
            background-color: #000000;
            text-decoration: none;
            border-radius: 5px;
        }}
        .product-image {{
            width: 50%;
            max-width: 50%;
            height: auto;
            margin: 20px auto !important;
        }}
        .img {{
            display: flex;
            width: 100%;
            align-items: center;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>We Have New Course!</h1>
        </div>
        <div class='content'>
            <h2>Dear Subscriber,</h2>
            <p>We are thrilled to introduce our latest product and we need your valuable feedback to make it even better!</p>
            
            <p>{course.Name}</p>
            <p>Click the button below to get started:</p>
            <a href='{url}' class='button'>Shop Now</a>
            <p>Thank you for being a valued subscriber and for your continued support.</p>
            <p>Best regards,</p>
            <p>The Edu Home's Team</p>
        </div>
        <div class='footer'>
            <p>&copy; 2024 Edu Home. All rights reserved.</p>
        </div>
    </div>
</body>
</html>
";


        foreach (var subscribe in subscribes)
        {
            _emailService.SendEmail(new(subscribe.Email, "New Course Info", emailBody));
        }


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
        var course = await _context.Courses.Include(x => x.CourseCategories).ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);

        if (course is null)
            return NotFound();


        return View(course);
    }

}
