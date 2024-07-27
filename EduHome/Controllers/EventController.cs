using EduHome.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Controllers;

public class EventController:Controller
{
    private readonly AppDbContext _context;

    public EventController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var events=await _context.Events.ToListAsync();

        return View(events);    
    }

    public async Task<IActionResult> Detail(int id)
    {
        var @event=await _context.Events.Include(x=>x.EventSpeakers).ThenInclude(x=>x.Speaker).FirstOrDefaultAsync(x=>x.Id==id);

        if (@event is null)
            return NotFound();

        return View(@event);
    }
}
