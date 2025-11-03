using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class FinalGradeRequestDto
    {
        [Range(0, 10, ErrorMessage = "La nota debe estar entre 0 y 10.")]
        public int FinalGrade { get; set; }
    }
}
