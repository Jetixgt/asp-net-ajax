using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspnetWebApp.Models;

namespace aspnetWebApp.Pages;

public class ResumeModel : PageModel
{
    public Student? Resume { get; set; }
    public IActionResult OnGet(int id)
    {
        Resume = CreateResumeModel.Students
        .FirstOrDefault(student => student.Id == id);
        if (Resume == null)
        {
            return NotFound();
        }
        return Page();
    }
}