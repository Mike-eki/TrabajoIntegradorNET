using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    internal class AcademicRepository
    {
        public List<Specialty> GetSpecialties()
        {
            return Data.InMemory.specialtiesSample;
        }
    }
}
