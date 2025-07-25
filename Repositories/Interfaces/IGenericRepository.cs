using Models.Entities;
using System.Collections.Generic;

namespace Repositories.Interfaces
{
    internal interface IGenericRepository
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
