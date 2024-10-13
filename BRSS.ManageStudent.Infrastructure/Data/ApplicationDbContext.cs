using BRSS.ManageStudent.Domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BRSS.ManageStudent.Infrastructure.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasMany(s => s.Classes)
            .WithMany(c => c.Students)
            .UsingEntity(x=>x.ToTable("StudentClass"));
        base.OnModelCreating(modelBuilder); 
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    DbSet<Student> Students { get; set; }
    DbSet<Class> Classes { get; set; }
}