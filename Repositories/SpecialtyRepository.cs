using Models.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    internal class SpecialtyRepository : ISpecialtyRepository
    {
        public List<Specialty> GetSpecialties()
        {
            return Data.InMemory.specialtiesSample;
        }
    }
}
