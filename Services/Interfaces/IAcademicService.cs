using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IAcademicService
    {
        List<Course> GetCourses();
        void CreateCourse(Course course);
        void UpdateCourse(Course course);
        void DeleteCourse(int id);
        List<Specialty> GetSpecialties();
        void CreateSpecialty(Specialty specialty);

    }
}
