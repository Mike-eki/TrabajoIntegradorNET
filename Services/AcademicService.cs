using Models.Entities;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AcademicService : IAcademicService
    {
        private readonly IAcademicRepository _academicRepository;

        public AcademicService(IAcademicRepository academicRepository)
        {
            _academicRepository = academicRepository;
        }
        public List<Course> GetCourses() 
        {
            //return _academicRepository.GetCourses().ForEach(c => c.SpecialtiesLinked = c.SpecialtiesLinked.OrderBy(s => s.Name).ToList());
            return _academicRepository.GetCourses();
        }

        public void UpdateCourse(Course existingcourse)
        {
            _academicRepository.UpdateCourse(existingcourse);
        }
    }
}
