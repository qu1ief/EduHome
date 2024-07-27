using EduHome.Contexts;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {

        var courses = await _context.Courses.Take(3).ToListAsync();

        var events=await _context.Events.Take(8).ToListAsync();

        var blogs=await _context.Blogs.Take(3).ToListAsync();


        HomeVm vm= new HomeVm()
        {
            Blogs= blogs,
            Events= events,
            Courses = courses
        };

        return View(vm);
    }

    public async Task<IActionResult> SubscribeEmail(string email,string returnUrl)
    {
        if (!ModelState.IsValid)
            return Redirect(returnUrl);

        var isExist = await _context.Subscribes.AnyAsync(x => x.Email.ToLower().Trim() == email.ToLower().Trim());

        if(isExist)
            return Redirect(returnUrl);


        Subscribe subscribe = new() {  Email = email };

        await _context.Subscribes.AddAsync(subscribe);
        await _context.SaveChangesAsync();  

        return Redirect(returnUrl);


    }
}
