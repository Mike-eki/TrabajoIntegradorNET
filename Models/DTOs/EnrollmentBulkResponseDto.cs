using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class EnrollmentBulkResponseDto
    {
        public int CreatedCount { get; set; }
        public int SkippedCount { get; set; }
        public int RemainingSeats { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

