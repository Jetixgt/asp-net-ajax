using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using aspnetWebApp.Models;
using System.Text.Json;

namespace aspnetWebApp.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext (
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {  
    }
    public DbSet<Student> Students { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Student>()
            .Property(student => student.Technologies)
            .HasConversion(
                technologies => JsonSerializer.Serialize(
                    technologies,
                    (JsonSerializerOptions?)null
                ),
                technologies =>JsonSerializer.Deserialize<string[]>(
                    technologies,
                    (JsonSerializerOptions?)null
                ) ?? Array.Empty<string>()
            );
        builder.Entity<Student>()
        .HasOne(student => student.User)
        .WithMany()
        .HasForeignKey(student => student.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }

}

