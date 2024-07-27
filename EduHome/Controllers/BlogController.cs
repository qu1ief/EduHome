using EduHome.Contexts;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class BlogController : Controller
{
    private readonly AppDbContext _context;

    public BlogController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(int? categoryId, string? search, int page = 1)
    {
        int pageCount = (int)Math.Ceiling((decimal)_context.Blogs.Count() / 3);

        if (page > pageCount)
            page = pageCount;

        if (page <= 0)
            page = 1;


        var query = _context.Blogs.Skip((page - 1) * 3).Take(3);

        if (search is not null)
            query = query.Where(x => x.Title.Contains(search) || x.Description.Contains(search));

        if(categoryId is not null)
            query=query.Where(x=>x.BlogCategoryId==categoryId);

        var blogs = await query.ToListAsync();

        ViewBag.PageCount=pageCount;
        ViewBag.CurrentPage=page;

        return View(blogs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var blog = await _context.Blogs.FirstOrDefaultAsync(x => x.Id == id);

        if(blog is null)
            return NotFound();

        var categories = await _context.BlogCategories.Include(x=>x.Blogs).ToListAsync();

        BlogDetailVm vm=new BlogDetailVm()
        {
            Blog=blog,
            BlogCategories=categories
        };

        return View(vm);
    }

}
