using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SubjectCreateDto
    {
        public string Name { get; set; } = null!;
        public int[] CareerIds { get; set; } = []; 

    }
}
