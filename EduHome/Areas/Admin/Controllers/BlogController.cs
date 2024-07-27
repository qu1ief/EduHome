using EduHome.Contexts;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class BlogController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _imagePath;

    public BlogController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
    }


    public async Task<IActionResult> Index()
    {
        var blogs = await _context.Blogs.ToListAsync();

        return View(blogs);
    }

    public async Task<IActionResult> Create()
    {
        var categories = await _context.BlogCategories.ToListAsync();

        ViewBag.Categories = categories;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(BlogCreateVm vm)
    {
        var categories = await _context.BlogCategories.ToListAsync();
        ViewBag.Categories = categories;

        if (!ModelState.IsValid)
            return View(vm);


        var isExistCategory = await _context.BlogCategories.AnyAsync(x => x.Id == vm.BlogCategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("BlogCategoryId", "Category is not found");
            return View(vm);
        }

        if (!vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please enter valid input");
            return View(vm);
        }


        string filename = await vm.Image.CreateFileAsync(_imagePath);

        Blog blog = new()
        {
            Title = vm.Title,
            Description = vm.Description,
            Author = vm.Author,
            CreatedTime = DateTime.Now,
            BlogCategoryId = vm.BlogCategoryId,
            ImagePath = filename
        };


        await _context.Blogs.AddAsync(blog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Detail(int id)
    {

        var blog = await _context.Blogs.Include(x => x.BlogCategory).FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();


        return View(blog);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();

        blog.ImagePath.DeleteFile(_imagePath);


        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

        if (blog is null)
            return NotFound();

        BlogUpdateVm vm = new()
        {
            Id = id,
            Author = blog.Author,
            Description = blog.Description,
            ImagePath = blog.ImagePath,
            BlogCategoryId = blog.BlogCategoryId,
            Title = blog.Title,
        };

        var categories = await _context.BlogCategories.ToListAsync();
        ViewBag.Categories = categories;

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(BlogUpdateVm vm)
    {
        var categories = await _context.BlogCategories.ToListAsync();
        ViewBag.Categories = categories;


        if (!ModelState.IsValid)
            return View(vm);

        var existBlog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existBlog is null)
            return BadRequest();


        var isExistCategory = await _context.Blogs.AnyAsync(x => x.Id == vm.BlogCategoryId);

        if (!isExistCategory)
        {
            ModelState.AddModelError("BlogCategoryId", "Category is not found");
            return View(vm);
        }


        if (vm.Image is { } && !vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid image");
            return View(vm);
        }


        existBlog.Title = vm.Title;
        existBlog.Description = vm.Description;
        existBlog.Author = vm.Author;
        existBlog.BlogCategoryId = vm.BlogCategoryId;

        if (vm.Image is { })
        {
            existBlog.ImagePath.DeleteFile(_imagePath);

            existBlog.ImagePath = await vm.Image.CreateFileAsync(_imagePath);
        }

        _context.Blogs.Update(existBlog);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


}
