using Models.Entities;
using Models.Enums;

namespace Data
{
    public class InMemory
    {
        public static List<User> usersSample;
        public static List<Specialty> specialtiesSample;
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
                new User { Id = 1, Username = "alicebeltran", Password = "student123", RoleName = RoleType.Student , Email = "mail@example.com", Name = "Alice Beltran" },
                new User { Id = 2, Username = "oscarwilde", Password = "proffesor123", RoleName = RoleType.Professor , Email = "test@example.com", Name = "Oscar Wilde" },
                new User { Id = 3, Username = "adminuser", Password = "admin123", RoleName = RoleType.Administrator , Email = "admin@admin.com", Name = "Miqueas Moreno" },
                new User { Id = 4, Username = "carlamatilde", Password = "proffesor123", RoleName = RoleType.Professor , Email = "t@t.com", Name = "Carla Matilde" },
            };

            // Sample specialties

            specialtiesSample = new List<Specialty>
            {
                new Specialty { Id = 1, Name = "Ingenieria en Sistemas"},
                new Specialty { Id = 2, Name = "Ingenieria Química"},
                new Specialty { Id = 3, Name = "Ingenieria Mecanica"},
                new Specialty { Id = 4, Name = "Ingenieria Civil"},
                new Specialty { Id = 5, Name = "Ingenieria Electrónica"},
                new Specialty { Id = 6, Name = "Ingenieria Industrial" },
                new Specialty { Id = 7, Name = "Ingenieria en Telecomunicaciones"},
                new Specialty { Id = 8, Name = "Ingenieria Ambiental"},
                new Specialty { Id = 9, Name = "Ingenieria en Energias Renovables"},
                new Specialty { Id = 10, Name = "Ingenieria en Inteligencia Artificial"},
                new Specialty { Id = 11, Name = "Ingenieria en Ciberseguridad"},
                new Specialty { Id = 12, Name = "Ingenieria en Robótica"},
                new Specialty { Id = 13, Name = "Ingenieria en Big Data"},
                new Specialty { Id = 14, Name = "Ingenieria en Desarrollo de Software"},
                new Specialty { Id = 15, Name = "Ingenieria en Sistemas Embebidos"},
                new Specialty { Id = 16, Name = "Ingenieria en Blockchain"},
                new Specialty { Id = 17, Name = "Ingenieria en Realidad Aumentada"},
            };

            // Sample courses

            coursesSample = new List<Course>
            {
                new Course { Id = 1, Name = "Programación I", CurricularPlanId = 1 },
                new Course { Id = 2, Name = "Programación II", CurricularPlanId = 1 },
                new Course { Id = 3, Name = "Bases de Datos", CurricularPlanId = 1 },
                new Course { Id = 4, Name = "Redes de Computadoras", CurricularPlanId = 1 },
                new Course { Id = 5, Name = "Matemáticas Discretas", CurricularPlanId = 1 },
                new Course { Id = 6, Name = "Algoritmos y Estructuras de Datos", CurricularPlanId = 1 },
                new Course { Id = 7, Name = "Sistemas Operativos", CurricularPlanId = 1 },
                new Course { Id = 8, Name = "Ingeniería de Software", CurricularPlanId = 1 },
                new Course { Id = 9, Name = "Inteligencia Artificial", CurricularPlanId = 1 },
                new Course { Id = 10, Name = "Ciberseguridad", CurricularPlanId = 1 },
                new Course { Id = 11, Name = "Desarrollo Web", CurricularPlanId = 1 },
                new Course { Id = 12, Name = "Desarrollo Móvil", CurricularPlanId = 1 },
                new Course { Id = 13, Name = "Computación en la Nube", CurricularPlanId = 1 },
                new Course { Id = 14, Name = "Big Data", CurricularPlanId = 1 },
                new Course { Id = 15, Name = "Blockchain", CurricularPlanId = 1 },
                new Course { Id = 16, Name = "Realidad Aumentada", CurricularPlanId = 1 },
                new Course { Id = 17, Name = "Robótica", CurricularPlanId = 1 },
                new Course { Id = 18, Name = "Sistemas Embebidos", CurricularPlanId = 1 },
                new Course { Id = 19, Name = "Ingeniería de Datos", CurricularPlanId = 1 },
                new Course { Id = 20, Name = "Desarrollo de Videojuegos", CurricularPlanId = 1 }
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
                new Commission { Id = 1, Name = "Comisión A", Shift = "Morning", Day = "Monday", Schedule = "8:00-10:00", CourseId = 1, ProfessorId = 2, Enabled = true,StudentLimit = 30 },
                new Commission { Id = 2, Name = "Comisión B", Shift = "Afternoon", Day = "Friday", Schedule = "14:00-16:00", CourseId = 2,  ProfessorId = 2, Enabled = true, StudentLimit = 25 },
                new Commission { Id = 3, Name = "Comisión C", Shift = "Evening", Day = "Tuesday", Schedule = "18:00-20:00", CourseId = 3,  ProfessorId = 4, Enabled = true, StudentLimit = 20 },
                new Commission { Id = 4, Name = "Comisión D", Shift = "Morning", Day = "Wednesday", Schedule = "10:00-12:00", CourseId = 4,  ProfessorId = 2, Enabled = true, StudentLimit = 15 },
                new Commission { Id = 5, Name = "Comisión E", Shift = "Afternoon", Day = "Thursday", Schedule = "16:00-18:00", CourseId = 5,  ProfessorId = 4, Enabled = true, StudentLimit = 10 },
                new Commission { Id = 6, Name = "Comisión F", Shift = "Evening", Day = "Friday", Schedule = "20:00-22:00", CourseId = 6,  ProfessorId = 2, Enabled = true, StudentLimit = 5 },
                new Commission { Id = 7, Name = "Comisión G", Shift = "Morning", Day = "Monday", Schedule = "8:00-10:00", CourseId = 7,  ProfessorId = 4, Enabled = true, StudentLimit = 30 },
                new Commission { Id = 8, Name = "Comisión H", Shift = "Afternoon", Day = "Tuesday", Schedule = "14:00-16:00", CourseId = 8,  ProfessorId = 2, Enabled = true, StudentLimit = 25 },
                new Commission { Id = 9, Name = "Comisión I", Shift = "Evening", Day = "Wednesday", Schedule = "18:00-20:00", CourseId = 9,  ProfessorId = 4, Enabled = true, StudentLimit = 20 },
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
