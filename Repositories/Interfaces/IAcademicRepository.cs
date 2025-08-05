using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAcademicRepository
    {
        List<Course> GetCourses();
        Course GetCourse(int id);
        void CreateCourse(Course course);
        void DeleteCourse(int id);
        void AddCourse(Course course);
        void UpdateCourse(Course course);

        List<Specialty> GetSpecialties();
        void AddSpecialty(Specialty specialty);

    }
}
