using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Mapea tus clases a tablas en la base de datos
        public DbSet<Career> Careers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Career → Subject (N:N)
            modelBuilder.Entity<Career>()
                .HasMany(c => c.Subjects)
                .WithMany(s => s.Careers)   // tabla intermedia automática
                .UsingEntity<Dictionary<string, object>>(
                "CareerSubject",
                    j => j.HasOne<Subject>().WithMany().HasForeignKey("SubjectId"),
                    j => j.HasOne<Career>().WithMany().HasForeignKey("CareerId"));

            // Subject → Commission (1:N)
            modelBuilder.Entity<Commission>()
                .HasOne(c => c.Subject)
                .WithMany(s => s.Commissions)
                .HasForeignKey(c => c.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Commission → Enrollment (1:N)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Commission)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CommissionId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
