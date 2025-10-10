using Microsoft.EntityFrameworkCore;
using Models;
using System;

namespace Repositories.EF
{
    public class EFAcademicRepository
    {
        private readonly AppDbContext _context;

        public EFAcademicRepository(AppDbContext context) 
        {
            _context = context;
        }

    }
}
