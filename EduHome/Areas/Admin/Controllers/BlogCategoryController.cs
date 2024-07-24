using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
public class BlogCategoryController : Controller
{
    private readonly AppDbContext _context;

    public BlogCategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var blogBlogCategories = await _context.BlogCategories.ToListAsync();

        return View(blogBlogCategories);

    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(BlogCategoryCreateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var isExist = await _context.BlogCategories.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower().Trim());

        if (isExist)
        {
            ModelState.AddModelError("Name", "This name is already exist");
            return View(vm);
        }

        BlogCategory BlogCategory = new()
        {
            Name = vm.Name,
        };

        await _context.BlogCategories.AddAsync(BlogCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var BlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);

        if (BlogCategory is null)
            return NotFound();


        _context.BlogCategories.Remove(BlogCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var BlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == id);


        if (BlogCategory is null)
            return NotFound();


        BlogCategoryUpdateVm vm = new()
        {
            Id = id,
            Name = BlogCategory.Name,
        };


        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(BlogCategoryUpdateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var existBlogCategory = await _context.BlogCategories.FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existBlogCategory is null)
            return NotFound();

        var isExist = await _context.BlogCategories.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower().Trim() && x.Id != vm.Id);

        if (isExist)
        {
            ModelState.AddModelError("Name", "Name is already exist");
            return View(vm);
        }

        existBlogCategory.Name = vm.Name;

        _context.BlogCategories.Update(existBlogCategory);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}