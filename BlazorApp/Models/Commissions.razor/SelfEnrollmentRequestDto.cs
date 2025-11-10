using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Models.Commissions.razor
{
    public class SelfEnrollmentRequestDto
    {
        public int StudentId { get; set; }
        public int CommissionId { get; set; }
    }
}
