using EduHome.Contexts;
using EduHome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]

public class SettingController : Controller
{
    private readonly AppDbContext _context;

    public SettingController(AppDbContext context)
    {
        _context = context;
    }


    public async Task<IActionResult> Index()
    {
        var settings = await _context.Settings.ToListAsync();


        return View(settings);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Setting setting)
    {
        if (!ModelState.IsValid)
            return View(setting);

        var isExist = await _context.Settings.AnyAsync(x => x.Key == setting.Key);

        if (isExist)
        {
            ModelState.AddModelError("Key", "This key is already exist");
            return View(setting);
        }

        await _context.Settings.AddAsync(setting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Update(int id)
    {
        var setting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == id);

        if (setting is null)
            return NotFound();

        return View(setting);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Setting setting)
    {
        if (!ModelState.IsValid)
            return View(setting);

        var existSetting = await _context.Settings.FirstOrDefaultAsync(x => x.Id == setting.Id);

        if (existSetting is null)
            return BadRequest();

        var isExist = await _context.Settings.AnyAsync(x => x.Key == setting.Key && x.Id != setting.Id);

        if (isExist)
        {
            ModelState.AddModelError("Key", "This key is already exist");
            return View(setting);
        }

        existSetting.Key = setting.Key;
        existSetting.Value = setting.Value;

        _context.Settings.Update(existSetting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Delete(int id)
    {

        var setting=await _context.Settings.FirstOrDefaultAsync(x=>x.Id==id);

        if(setting is null) 
            return NotFound();


        _context.Settings.Remove(setting);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

}
