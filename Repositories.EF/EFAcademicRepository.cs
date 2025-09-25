using Microsoft.EntityFrameworkCore;
using Models;
using Repositories;
using System;

namespace Repositories.EF
{
    public class EFAcademicRepository
    {
        private readonly AppDbContext _context;

        public EFCommissionRepository(AppDbContext context)
        {
            _context = context;
        }

    }
}
