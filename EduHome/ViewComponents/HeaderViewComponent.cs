using EduHome.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public HeaderViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

        return View(settings);
    }
}


public class SubscribeViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        
        return View();
    }
}
