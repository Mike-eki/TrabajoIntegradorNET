using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class EnrollmentSimpleDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public string Status { get; set; } = null!;
    }
}
