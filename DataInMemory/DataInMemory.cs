using Models.Entities;

namespace Data
{
    public class InMemory
    {
        public static List<User> usersSample;
        public static List<Specialty> specialtiesSample;
        public static List<Role> rolesSample;
        public static List<Course> coursesSample;
        public static List<CurricularPlan> curricularPlansSample;
        public static List<Commission> commissionsSample;
        public static List<Enrollment> enrollmentsSample;
        public static List<Grade> gradesSample;

        static InMemory() 
        {
            // Sample users

            usersSample = new List<User>
            {
                new User { Id = 1, Username = "alicebeltran", Password = "student123", RoleId = 1, Email = "mail@example.com", Name = "Alice Beltran" },
                new User { Id = 2, Username = "oscarwilde", Password = "proffesor123", RoleId = 2, Email = "test@example.com", Name = "Oscar Wilde" },
            };

            // Same roles

            rolesSample = new List<Role>
            {
                new Role { Id = 1, Name = "Student" },
                new Role { Id = 2, Name = "Professor" },
                new Role { Id = 3, Name = "Administrator" }
            };

            // Sample specialties

            specialtiesSample = new List<Specialty>
            {
                new Specialty { Id = 1, Name = "Ingenieria en Sistemas"},
                new Specialty { Id = 2, Name = "Ingenieria Química"},
                new Specialty { Id = 3, Name = "Ingenieria Mecanica"},
                new Specialty { Id = 4, Name = "Ingenieria Civil"}
            };

            // Sample courses

            coursesSample = new List<Course>
            {
                new Course { Id = 1, Name = "Programación I", CurricularPlanId = 1 },
                new Course { Id = 2, Name = "Programación II", CurricularPlanId = 1 },
                new Course { Id = 3, Name = "Bases de Datos", CurricularPlanId = 1 },
                new Course { Id = 4, Name = "Redes de Computadoras", CurricularPlanId = 1 },
                new Course { Id = 5, Name = "Matemáticas Discretas", CurricularPlanId = 1 }
            };

            // Sample curricular plans

            curricularPlansSample = new List<CurricularPlan>
            {
                new CurricularPlan { Id = 1, PlanName = "Plan 2023", CourseId = 1, AcademicPeriod = "anual" },
                new CurricularPlan { Id = 2, PlanName = "Plan 2023", CourseId = 2, AcademicPeriod = "semester" },
            };


            // Sample commissions

            commissionsSample = new List<Commission>
            {
                new Commission { Id = 1, Name = "Comisión A", Shift = "Morning", Day = "Monday", Schedule = "8:00-10:00", CourseId = 1, Enabled = true, StudentLimit = 30 },
                new Commission { Id = 2, Name = "Comisión B", Shift = "Afternoon", Day = "Friday", Schedule = "14:00-16:00", CourseId = 2, Enabled = true, StudentLimit = 25 },
                new Commission { Id = 3, Name = "Comisión C", Shift = "Evening", Day = "Tuesday", Schedule = "18:00-20:00", CourseId = 3, Enabled = true, StudentLimit = 20 }
            };


            // Sample enrollments

            enrollmentsSample = new List<Enrollment>
            {
                //new Enrollment { Id = 1, UserId = 1, CommissionId = 1, EnrollDate = new DateTime(2023, 03, 15) },
                //new Enrollment { Id = 2, UserId = 1, CommissionId = 2, EnrollDate = new DateTime(2023, 08, 20) },
                //new Enrollment { Id = 3, UserId = 2, CommissionId = 3, EnrollDate = new DateTime(2024, 03, 01) }
            };


            // Sample grades

            gradesSample = new List<Grade>
            {
                //new Grade { Id = 1, EnrollmentId = 1, Mark = 85, Date = new DateTime(2023, 06, 15) },
                //new Grade { Id = 2, EnrollmentId = 2, Mark = 90, Date = new DateTime(2023, 11, 20) },
                //new Grade { Id = 3, EnrollmentId = 3, Mark = 78, Date = new DateTime(2024, 04, 10) }
            };
        }
    }
}
