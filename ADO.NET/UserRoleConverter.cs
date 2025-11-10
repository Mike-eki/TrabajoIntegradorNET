using Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET
{
    public static class UserRoleConverter
    {
        public static string ToString(UserRole role) => role switch
        {
            UserRole.Admin => "Admin",
            UserRole.Student => "Student",
            UserRole.Professor => "Professor",
            _ => throw new ArgumentException($"Invalid role: {role}", nameof(role))
        };

        public static UserRole FromString(string role) => role switch
        {
            "Admin" => UserRole.Admin,
            "Student" => UserRole.Student,
            "Professor" => UserRole.Professor,
            _ => throw new ArgumentException($"Invalid role string: {role}", nameof(role))
        };
    }
}
