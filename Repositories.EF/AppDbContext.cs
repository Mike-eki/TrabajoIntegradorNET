using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Models.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<UserSpecialty> UserSpecialties { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ===== 1. CONFIGURACIÓN DE PROPIEDADES Y VALIDACIONES =====

            modelBuilder.Entity<Specialty>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.AcademicPeriod).IsRequired();
                // Configurar la relación muchos a muchos con Specialty
            });

            modelBuilder.Entity<Commission>(entity =>
            {
                entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Day).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Schedule).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Enabled).IsRequired();
                entity.Property(e => e.MaxEnrolls).IsRequired();
                entity.HasOne(e => e.Professor)
                      .WithMany()
                      .HasForeignKey(e => e.ProfessorId)
                      .OnDelete(DeleteBehavior.SetNull);
                entity.HasOne(e => e.Course)
                      .WithMany()
                      .HasForeignKey(e => e.CourseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ===== 2. CONFIGURACIÓN DE RELACIONES COMPLEJAS =====

            // User -> Enrollments
            modelBuilder.Entity<User>()
                .HasMany(u => u.Enrollments)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.UserId);

            // User -> UserSpecialties
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserSpecialties)
                .WithOne(us => us.User)
                .HasForeignKey(us => us.UserId);

            // Commission -> Enrollments (ya configurado arriba en Commission, pero aseguramos la relación inversa)
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Commission)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CommissionId);

            // Enrollment -> Grade
            modelBuilder.Entity<Enrollment>()
                .HasMany(e => e.Grades)
                .WithOne(g => g.Enrollment)
                .HasForeignKey(g => g.EnrollmentId);

            // UserSpecialty -> User y Specialty
            modelBuilder.Entity<UserSpecialty>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserSpecialties)
                .HasForeignKey(us => us.UserId);

            modelBuilder.Entity<UserSpecialty>()
                .HasOne(us => us.Specialty)
                .WithMany(s => s.UserSpecialties)
                .HasForeignKey(us => us.SpecialtyId);

            // Specialty <-> Course (muchos a muchos)
            modelBuilder.Entity<Specialty>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Specialties)
                .UsingEntity("SpecialtyCourses");


            base.OnModelCreating(modelBuilder);
        }
    }
}
