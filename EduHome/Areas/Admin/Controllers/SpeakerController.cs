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

public class SpeakerController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _imagePath;

    public SpeakerController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
    }

    public async Task<IActionResult> Index()
    {
        var speaker = await _context.Speakers.ToListAsync();

        return View(speaker);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(SpeakerCreateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        if (!vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid Image");
            return View(vm);
        }

        string filename = await vm.Image.CreateFileAsync(_imagePath);

        Speaker speaker = new()
        {
            Fullname = vm.Fullname,
            Profession = vm.Profession,
            ImagePath = filename

        };

        await _context.Speakers.AddAsync(speaker);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");

    }

    public async Task<IActionResult> Update(int id)
    {
        var speaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == id);

        if (speaker is null)
            return NotFound();


        SpeakerUpdateVm vm = new()
        {
            Id = id,
            Fullname = speaker.Fullname,
            Profession = speaker.Profession,
            ImagePath = speaker.ImagePath,
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Update(SpeakerUpdateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existSpeaker = await _context.Speakers.FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existSpeaker is null)
            return BadRequest();


        if (vm.Image is {} && !vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid image");
            return View(vm);
        }

        existSpeaker.Fullname = vm.Fullname;
        existSpeaker.Profession = vm.Profession;

        if(vm.Image is {})
        {
            existSpeaker.ImagePath.DeleteFile(_imagePath);

            existSpeaker.ImagePath=await vm.Image.CreateFileAsync(_imagePath);
        }

        _context.Speakers.Update(existSpeaker);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Delete(int id)
    {
        var speaker=await _context.Speakers.FirstOrDefaultAsync(x=>x.Id==id);

        if(speaker is null)
            return NotFound();

        _context.Speakers.Remove(speaker);
        await _context.SaveChangesAsync();

        speaker.ImagePath.DeleteFile(_imagePath);

        return RedirectToAction("Index");

    }
}
