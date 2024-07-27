using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class CategoryController : Controller
{

    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var categories = await _context.Categories.ToListAsync();

        return View(categories);
    }


    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CategoryCreateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var isExist = await _context.Categories.AnyAsync(x => x.Name.ToLower() == vm.Name.ToLower().Trim());

        if (isExist)
        {
            ModelState.AddModelError("Name", "This name is already exist");
            return View(vm);
        }

        Category category = new()
        {
            Name = vm.Name,
        };

        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var category=await _context.Categories.FirstOrDefaultAsync(x=>x.Id== id);   

        if(category is null)
            return NotFound();


        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var category=await _context.Categories.FirstOrDefaultAsync(x=>x.Id==id);


        if (category is null)
            return NotFound();


        CategoryUpdateVm vm = new()
        {
            Id = id,
            Name = category.Name,
        };


        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        var existCategory=await _context.Categories.FirstOrDefaultAsync(x=>x.Id==vm.Id);

        if(existCategory is null)
            return NotFound();

        var isExist=await _context.Categories.AnyAsync(x=>x.Name.ToLower()==vm.Name.ToLower().Trim() && x.Id!=vm.Id);

        if (isExist)
        {
            ModelState.AddModelError("Name", "Name is already exist");
            return View(vm);    
        }

        existCategory.Name = vm.Name;

        _context.Categories.Update(existCategory);  
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }
}
