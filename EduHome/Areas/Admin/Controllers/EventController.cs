using EduHome.Contexts;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
public class EventController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _imagePath;

    public EventController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
    }

    public async Task<IActionResult> Index()
    {
        var events = await _context.Events.ToListAsync();

        return View(events);
    }


    public async Task<IActionResult> Create()
    {
        var speakers = await _context.Speakers.ToListAsync();

        ViewBag.Speakers = speakers;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(EventCreateVm vm)
    {
        var speakers = await _context.Speakers.ToListAsync();
        ViewBag.Speakers = speakers;


        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid input");
            return View(vm);
        }

        foreach (var speakerId in vm.SpeakerIds)
        {
            var isExistSpeaker = await _context.Speakers.AnyAsync(x => x.Id == speakerId);

            if (!isExistSpeaker)
            {
                ModelState.AddModelError("SpeakerIds", "Speaker is not found");
                return View(vm);
            }
        }

        if (vm.Date < DateTime.Now)
        {
            ModelState.AddModelError("Date", "You cannot select the old date");
            return View(vm);
        }

        string filename = await vm.Image.CreateFileAsync(_imagePath);


        Event @event = new()
        {
            Name = vm.Name,
            Time = vm.Time,
            Venue = vm.Venue,
            Description = vm.Description,
            Date = vm.Date,
            EventSpeakers = new List<EventSpeaker>(),
            ImagePath = filename

        };

        vm.SpeakerIds.ForEach(x =>
        {
            EventSpeaker eventSpeaker = new()
            {
                Event = @event,
                SpeakerId = x
            };

            @event.EventSpeakers.Add(eventSpeaker);

        });

        await _context.Events.AddAsync(@event);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }





    public async Task<IActionResult> Delete(int id)
    {
        var @event = await _context.Events.FirstOrDefaultAsync(x => x.Id == id);

        if (@event is null)
            return NotFound();

        _context.Events.Remove(@event);
        await _context.SaveChangesAsync();

        @event.ImagePath.DeleteFile(_imagePath);

        return RedirectToAction("Index");
    }


    public async Task<IActionResult> Detail(int id)
    {
        var @event = await _context.Events.Include(x => x.EventSpeakers).ThenInclude(x => x.Speaker).FirstOrDefaultAsync(x => x.Id == id);


        if (@event is null)
            return NotFound();


        return View(@event);
    }


    public async Task<IActionResult> Update(int id)
    {
        var @event = await _context.Events.Include(x => x.EventSpeakers).FirstOrDefaultAsync(x => x.Id == id);

        if (@event is null)
            return NotFound();

        EventUpdateVm vm = new()
        {
            Id = id,
            Name = @event.Name,
            Date = @event.Date,
            Description = @event.Description,
            ImagePath = @event.ImagePath,
            SpeakerIds = @event.EventSpeakers.Select(x => x.SpeakerId).ToList(),
            Time = @event.Time,
            Venue = @event.Venue

        };

        var speakers = await _context.Speakers.ToListAsync();

        ViewBag.Speakers = speakers;

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(EventUpdateVm vm)
    {
        var speakers = await _context.Speakers.ToListAsync();
        ViewBag.Speakers = speakers;

        if (!ModelState.IsValid)
            return View(vm);

        var existEvent = await _context.Events.Include(x=>x.EventSpeakers).FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existEvent is null)
            return BadRequest();

        if (vm.Image is { } && !vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid input");
            return View(vm);
        }


        foreach (var speakerId in vm.SpeakerIds)
        {
            var isExistSpeaker = await _context.Speakers.AnyAsync(x => x.Id == speakerId);

            if (!isExistSpeaker)
            {
                ModelState.AddModelError("SpeakerIds", "Speaker is not found");
                return View(vm);
            }
        }

        if (vm.Date < DateTime.Now)
        {
            ModelState.AddModelError("Date", "You cannot select the old date");
            return View(vm);
        }


        existEvent.Name = vm.Name;
        existEvent.Description = vm.Description;
        existEvent.Date = vm.Date;
        existEvent.Time = vm.Time;
        existEvent.Venue = vm.Venue;
        existEvent.EventSpeakers = new List<EventSpeaker>();

        if (vm.Image is { })
        {
            existEvent.ImagePath.DeleteFile(_imagePath);
            existEvent.ImagePath = await vm.Image.CreateFileAsync(_imagePath);
        }

        vm.SpeakerIds.ForEach(x =>
        {
            EventSpeaker eventSpeaker = new EventSpeaker()
            {
                SpeakerId=x,
                 EventId=vm.Id,
                  Event=existEvent,

            };

            existEvent.EventSpeakers.Add(eventSpeaker);

        });

        _context.Events.Update(existEvent);
        await _context.SaveChangesAsync();


        return RedirectToAction("Index");

    }

}
