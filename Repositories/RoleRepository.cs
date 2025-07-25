using Models.Entities;
using Repositories.Interfaces;
using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RoleRepository : IRoleRepository
    {
        public List<Role> GetRoles()
        {
            // Return the list of roles from the in-memory data
            return InMemory.rolesSample;
        }
    }
}
