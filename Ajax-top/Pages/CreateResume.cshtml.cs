using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspnetWebApp.Data;
using aspnetWebApp.Models;

namespace aspnetWebApp.Pages;

// создавать резюме могут только вошедшие пользователи
[Authorize]
public class CreateResumeModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public CreateResumeModel(ApplicationDbContext db)
    {
        _db = db;
    }

    [BindProperty]
    public string Name { get; set; } = "";
    [BindProperty]
    public string Phone { get; set; } = "";
    [BindProperty]
    public string Email { get; set; } = "";
    [BindProperty]
    public string Speciality { get; set; } = "";
    [BindProperty]
    public string Course { get; set; } = "";
    [BindProperty]
    public string BirthDate { get; set; } = "";
    [BindProperty]
    public string[] Technologies { get; set; } = Array.Empty<string>();

    private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var student = new Student
        {
            Name = Name,
            Phone = Phone,
            Email = Email,
            Speciality = Speciality,
            Course = Course,
            BirthDate = BirthDate,
            Technologies = Technologies,
            UserId = CurrentUserId
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return new JsonResult(ToDto(student));
    }

    // резюме текущего пользователя
    public async Task<IActionResult> OnGetStudentsAsync()
    {
        var students = await _db.Students
            .Where(student => student.UserId == CurrentUserId)
            .OrderBy(student => student.Id)
            .ToListAsync();

        return new JsonResult(students.Select(ToDto));
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var student = await _db.Students
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == CurrentUserId);
        if (student == null)
        {
            return new JsonResult(new
            {
                success = false,
                message = "Студент не найден"
            });
        }

        _db.Students.Remove(student);
        await _db.SaveChangesAsync();
        return new JsonResult(new
        {
            success = true
        });
    }

    private static object ToDto(Student s) => new
    {
        s.Id,
        s.Name,
        s.Phone,
        s.Email,
        s.Speciality,
        s.Course,
        s.BirthDate,
        s.Technologies
    };
}
