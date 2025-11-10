using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class SubjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public IEnumerable<CareerSimpleDto> Careers { get; set; } = new List<CareerSimpleDto>();

    }
}
