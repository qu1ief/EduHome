using EduHome.Contexts;
using EduHome.Helpers;
using EduHome.Models;
using EduHome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHome.Areas.Admin.Controllers;
[Area("Admin")]
public class TeacherController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private string _imagePath;

    public TeacherController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "img");
    }


    public async Task<IActionResult> Index()
    {
        var teachers = await _context.Teachers.ToListAsync();

        return View(teachers);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(TeacherCreateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);


        if (!vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid input");
            return View(vm);
        }

        string filename = await vm.Image.CreateFileAsync(_imagePath);

        Teacher teacher = new()
        {
            Degree = vm.Degree,
            Description = vm.Description,
            Email = vm.Email,
            Experience = vm.Experience,
            Faculty = vm.Faculty,
            Fullname = vm.Fullname,
            Hobbies = vm.Hobbies,
            PhoneNumber = vm.PhoneNumber,
            Profession = vm.Profession,
            Skype = vm.Skype,
            ImagePath = filename,
        };

        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Detail(int id)
    {
        var teacher = await _context.Teachers.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);

        if (teacher is null)
            return NotFound();

        return View(teacher);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == id);

        if (teacher is null)
            return NotFound();

        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();

        teacher.ImagePath.DeleteFile(_imagePath);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Update(int id)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);

        if (teacher is null)
            return NotFound();

        TeacherUpdateVm vm = new()
        {
            Id = id,
            Faculty = teacher.Faculty,
            Degree = teacher.Degree,
            Description = teacher.Description,
            Email = teacher.Email,
            Experience = teacher.Experience,
            Fullname = teacher.Fullname,
            Hobbies = teacher.Hobbies,
            ImagePath = teacher.ImagePath,
            PhoneNumber = teacher.PhoneNumber,
            Profession = teacher.Profession,
            Skype = teacher.Skype,

        };

        return View(vm);

    }

    [HttpPost]
    public async Task<IActionResult> Update(TeacherUpdateVm vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        var existTeacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (existTeacher is null)
            return BadRequest();

        if (vm.Image is { } && !vm.Image.ValidateImage())
        {
            ModelState.AddModelError("Image", "Please reenter valid input");
            return View(vm);
        }

        existTeacher.PhoneNumber = vm.PhoneNumber;
        existTeacher.Profession = vm.Profession;
        existTeacher.Email = vm.Email;
        existTeacher.Experience = vm.Experience;
        existTeacher.Faculty = vm.Faculty;
        existTeacher.Degree = vm.Degree;
        existTeacher.Description = vm.Description;
        existTeacher.Fullname = vm.Fullname;
        existTeacher.Skype = vm.Skype;
        existTeacher.Hobbies = vm.Hobbies;

        if (vm.Image is { })
        {
            existTeacher.ImagePath.DeleteFile(_imagePath);
            existTeacher.ImagePath = await vm.Image.CreateFileAsync(_imagePath);
        }

        _context.Teachers.Update(existTeacher);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> CreateSkill(int id)
    {
        var teacher = await _context.Teachers.FirstOrDefaultAsync(x => x.Id == id);

        if (teacher is null)
            return NotFound();


        ViewBag.Teacher = teacher;

        TeacherSkillVm vm = new TeacherSkillVm();
        vm.Id = id;

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSkill(TeacherSkillVm vm)
    {


        var teacher = await _context.Teachers.Include(x => x.Skills).FirstOrDefaultAsync(x => x.Id == vm.Id);

        if (teacher is null)
            return BadRequest();

        ViewBag.Teacher = teacher;

        if (!ModelState.IsValid)
            return View(vm);


        var isExist = teacher.Skills.Any(x => x.Name.ToLower().Trim() == vm.Name.ToLower().Trim());

        if (isExist)
        {
            ModelState.AddModelError("Name", "This skill is already exist");
            return View(vm);
        }

        Skill skill = new Skill()
        {
            Teacher = teacher,
            TeacherId = vm.Id,
            Name = vm.Name,
            Point = vm.Point,
        };

        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();


        return RedirectToAction("Detail", new { id = vm.Id });
    }

    public async Task<IActionResult> DeleteSkill(int id)
    {
        var skill = await _context.Skills.Include(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);

        if (skill is null)
            return NotFound();

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();


        return RedirectToAction("Detail", new { id = skill.Teacher.Id });
    }
}
