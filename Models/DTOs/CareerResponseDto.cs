using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class CareerResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<SubjectSimpleDto> Subjects { get; set; } = new List<SubjectSimpleDto>();
    }
}
