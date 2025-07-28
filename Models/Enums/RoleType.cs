using System.ComponentModel;

namespace Models.Enums
{
    public enum RoleType
    {
        [Description("Student")]
        Student,
        [Description("Professor")]
        Professor,
        [Description("Administrator")]
        Administrator
    }
}
