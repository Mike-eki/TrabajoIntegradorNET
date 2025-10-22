using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public record UserRequest(
        string Username,
        string Password,
        string Legajo,
        string Email,
        string Fullname,
        string Role = "Student"
    );
}
