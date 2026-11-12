using System.Security.Claims;
using aspnetWebApp.Data;
using aspnetWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace aspnetWebApp.Pages;

public class ResumesModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public ResumesModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public List<Student> Students { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string SpecialityFilter { get; set; } = string.Empty;

    public List<string> Specialities { get; set; } = new();

    public string? CurrentUserId { get; set; }

    public async Task OnGetAsync()
    {
        CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // SQLite не умеет сравнивать кириллицу без учёта регистра, поэтому фильтруем в памяти
        var students = await _db.Students.OrderBy(student => student.Id).ToListAsync();

        Specialities = students
            .Select(student => student.Speciality)
            .Where(speciality => !string.IsNullOrWhiteSpace(speciality))
            .Distinct()
            .OrderBy(speciality => speciality, StringComparer.CurrentCultureIgnoreCase)
            .ToList();

        Students = students
            .Where(student =>
                string.IsNullOrWhiteSpace(Search) ||
                student.Name.Contains(Search.Trim(), StringComparison.OrdinalIgnoreCase))
            .Where(student =>
                string.IsNullOrEmpty(SpecialityFilter) ||
                student.Speciality == SpecialityFilter)
            .ToList();
    }
}
