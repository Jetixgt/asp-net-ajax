using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using aspnetWebApp.Data;
using aspnetWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace aspnetWebApp.Pages;

// редактировать может только автор резюме
[Authorize]
public class EditResumeModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public EditResumeModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public Student? Resume { get; private set; }

    [BindProperty]
    [Required(ErrorMessage = "Укажите имя.")]
    public string Name { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Укажите номер телефона.")]
    public string Phone { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Укажите email.")]
    [EmailAddress(ErrorMessage = "Укажите корректный email.")]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Выберите специальность.")]
    public string Speciality { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Выберите курс.")]
    public string Course { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Укажите дату рождения.")]
    public string BirthDate { get; set; } = string.Empty;

    [BindProperty]
    public string[] Technologies { get; set; } = Array.Empty<string>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Resume = await FindResumeAsync(id);
        if (Resume is null)
        {
            return NotFound();
        }

        Name = Resume.Name;
        Phone = Resume.Phone;
        Email = Resume.Email;
        Speciality = Resume.Speciality;
        Course = Resume.Course;
        BirthDate = Resume.BirthDate;
        Technologies = Resume.Technologies;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Resume = await FindResumeAsync(id);
        if (Resume is null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Resume.Name = Name;
        Resume.Phone = Phone;
        Resume.Email = Email;
        Resume.Speciality = Speciality;
        Resume.Course = Course;
        Resume.BirthDate = BirthDate;
        Resume.Technologies = Technologies;
        await _db.SaveChangesAsync();

        return RedirectToPage("/Resume", new { id = Resume.Id });
    }

    // ищем резюме, принадлежащее текущему пользователю
    private Task<Student?> FindResumeAsync(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return _db.Students.FirstOrDefaultAsync(student => student.Id == id && student.UserId == userId);
    }
}
