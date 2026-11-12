using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using aspnetWebApp.Data;
using aspnetWebApp.Models;

namespace aspnetWebApp.Pages;

public class ResumeModel : PageModel
{
    private readonly ApplicationDbContext _db;

    public ResumeModel(ApplicationDbContext db)
    {
        _db = db;
    }

    public Student? Resume { get; set; }

    // может ли текущий пользователь редактировать это резюме
    public bool CanEdit { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Resume = await _db.Students.FirstOrDefaultAsync(student => student.Id == id);
        if (Resume == null)
        {
            return NotFound();
        }
        CanEdit = Resume.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Page();
    }
}
