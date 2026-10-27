using aspnetWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnetWebApp.Pages;

public class ResumesModel : PageModel
{
    public List<Student> Students { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string Search { get; set; } = string.Empty;

    [BindProperty(SupportsGet = true)]
    public string SpecialityFilter { get; set; } = string.Empty;

    public List<string> Specialities { get; set; } = new();

    public void OnGet()
    {
        var students = CreateResumeModel.Students;
        Specialities = students
            .Select(student => student.Speciality)
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
