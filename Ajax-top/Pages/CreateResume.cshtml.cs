// using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using aspnetWebApp.Models;

namespace aspnetWebApp.Pages;

public class CreateResumeModel : PageModel
{
    [BindProperty]
    public required string Name { get; set; }
    [BindProperty]
    public required string Phone { get; set; }
    [BindProperty]
    public required string Email { get; set; }
    [BindProperty]
    public required string Speciality { get; set; }
    [BindProperty]
    public required string Course { get; set; }
    [BindProperty]
    public required string BirthDate { get; set; }
    [BindProperty]
    public string[] Technologies { get; set; } = Array.Empty<string>();
    public static List<Student> Students { get; set; } = new();
    public required string Message { get; set; }
    public void OnGet()
    {
        // Message = "Привет! Сообщение от C#";
    }
    public IActionResult OnPost() {

        string technologies = Technologies.Length > 0
            ? string.Join(", ", Technologies)
            : "Не выбраны";

        // string message = $"Анкета студента\n\n" +
        //         $"Имя: {Name}\n" + 
        //         $"Телефон: {Phone}\n" +
        //         $"Email: {Email}\n" +
        //         $"Специальность: {Speciality}\n" +
        //         $"Курс: {Course}\n" +
        //         $"Дата рождения: {BirthDate}\n" +
        //         $"Технологии: {technologies}";

        var student = new Student
        {
            Id = Students.Any() ? Students.Max(x => x.Id) + 1 : 1,
            Name = Name,
            Phone = Phone,
            Email = Email,
            Speciality = Speciality,
            Course = Course,
            BirthDate = BirthDate,
            Technologies = Technologies
        };

        Students.Add(student);

        return new JsonResult(student);
    }

    public IActionResult OnGetStudents() {
        return new JsonResult(Students);
    }

    public IActionResult OnPostDelete(int id) {
        var student = Students.FirstOrDefault(x => x.Id == id);
        if (student == null) {
            return new JsonResult(new{
                success = false,
                message = "Студент не найден"
            });
        }
        Students.Remove(student);
        return new JsonResult(new{
            success = true
        });
    }
}
