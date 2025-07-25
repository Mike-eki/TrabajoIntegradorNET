using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class UpdateUserDTO
    {
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string? Email { get; set; }

        public string? Name { get; set; }
    }
}
