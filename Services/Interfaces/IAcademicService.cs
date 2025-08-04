using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Entities;

namespace Services.Interfaces
{
    public interface IAcademicService
    {
        List<Course> GetCourses();
        void UpdateCourse(Course course);
    }
}
