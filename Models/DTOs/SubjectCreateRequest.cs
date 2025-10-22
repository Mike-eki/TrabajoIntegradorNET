using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record SubjectCreateRequest(
            string Name,
            int[] CareerIds // <-- lista de IDs de carreras a las que pertenece
        );
}
