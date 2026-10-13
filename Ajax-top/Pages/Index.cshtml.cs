using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace aspnetWebApp.Pages;

public class IndexModel : PageModel
{
    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? Phone { get; set; }

    [BindProperty]
    public string? Email { get; set; }

    [BindProperty]
    public string? Speciality { get; set; }

    [BindProperty]
    public string? Course { get; set; }

    [BindProperty]
    public string? City { get; set; }

    [BindProperty]
    public string? MainLanguage { get; set; }

    [BindProperty]
    public string? BirthDate { get; set; }

    [BindProperty]
    public string[] Technologies { get; set; } = Array.Empty<string>();

    public string? Message { get; set; }

    public IActionResult OnPost()
    {
        string technologies = Technologies.Length > 0
            ? string.Join(", ", Technologies)
            : "Не выбраны";

        string message =
            $"Анкета студента\n\n" +
            $"Имя: {Name}\n" +
            $"Телефон: {Phone}\n" +
            $"Email: {Email}\n" +
            $"Специальность: {Speciality}\n" +
            $"Курс: {Course}\n" +
            $"Город: {City}\n" +
            $"Основной язык программирования: {MainLanguage}\n" +
            $"Дата рождения: {BirthDate}\n" +
            $"Технологии: {technologies}";

        Message = message;
        var json = new
        {
            Name = Name,
            Phone = Phone,
            Email = Email,
            Speciality = Speciality,
            Course = Course,
            City = City,
            MainLanguage = MainLanguage,
            BirthDate = BirthDate,
            Technologies = Technologies,
            Message = Message
        };

        return new JsonResult(json);
    }
}
