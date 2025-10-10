using Data;
using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class AcademicRepository : IAcademicRepository
    {
        public List<Course> GetCourses()
        {
            return InMemory.coursesSample;
        }
        public Course GetCourse(int id)
        {
            return InMemory.coursesSample.FirstOrDefault(c => c.Id == id)
            ?? throw new KeyNotFoundException("Curso no encontrado");
        }
        public void CreateCourse(Course course)
        {
            AddCourse(course);
        }
        public void DeleteCourse(int id)
        {
            var course = GetCourse(id);
            InMemory.coursesSample.Remove(course);
        }
        public void AddCourse(Course course)
        {
            if (InMemory.coursesSample.Any(c => c.Name.Equals(course.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("El curso ya existe.");
            }
            course.Id = InMemory.coursesSample.Max(c => c.Id) + 1; // Assign a new ID
            InMemory.coursesSample.Add(course);
        }
        public void UpdateCourse(Course course)
        {
            var existingCourse = GetCourse(course.Id);
            if (existingCourse == null)
            {
                throw new KeyNotFoundException("Curso no encontrado");
            }
            // Update properties
            existingCourse.Name = course.Name;
            existingCourse.AcademicPeriod = course.AcademicPeriod;
            existingCourse.Specialties = course.Specialties;
            //existingCourse.CurricularPlan = course.CurricularPlan;
            // No need to save changes in InMemory, as it's already updated in the list
        }

        public List<Specialty> GetSpecialties()
        {
            return InMemory.specialtiesSample;
        }

        public void AddSpecialty(Specialty specialty)
        {
            if (InMemory.specialtiesSample.Any(s => s.Name.Equals(specialty.Name, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("La especialidad ya existe.");
            }
            specialty.Id = InMemory.specialtiesSample.Max(s => s.Id) + 1; // Assign a new ID
            InMemory.specialtiesSample.Add(specialty);
        }
    }
}
