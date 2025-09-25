using Models.Entities;
using Models.Enums;

namespace Data
{
    public class InMemory
    {
        public static List<User> usersSample;
        public static List<Specialty> specialtiesSample;
        public static List<Course> coursesSample;
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
                new Course { Id = 1, Name = "Programación I" , AcademicPeriod = AcademicPeriodType.Year, SpecialtiesLinked = new List<Specialty>() { specialtiesSample.Find(s => s.Id == 1)} },
                new Course { Id = 2, Name = "Programación II" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 3, Name = "Bases de Datos" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 4, Name = "Redes de Computadoras"  , AcademicPeriod = AcademicPeriodType.Quarter},
                new Course { Id = 5, Name = "Matemáticas Discretas" , AcademicPeriod = AcademicPeriodType.Semester},
                new Course { Id = 6, Name = "Algoritmos y Estructuras de Datos", AcademicPeriod = AcademicPeriodType.Semester},
                new Course { Id = 7, Name = "Sistemas Operativos" , AcademicPeriod = AcademicPeriodType.Semester},
                new Course { Id = 8, Name = "Ingeniería de Software" , AcademicPeriod = AcademicPeriodType.Quarter},
                new Course { Id = 9, Name = "Inteligencia Artificial" , AcademicPeriod = AcademicPeriodType.Quarter},
                new Course { Id = 10, Name = "Ciberseguridad" , AcademicPeriod = AcademicPeriodType.Quarter},
                new Course { Id = 11, Name = "Desarrollo Web" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 12, Name = "Desarrollo Móvil" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 13, Name = "Computación en la Nube" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 14, Name = "Big Data" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 15, Name = "Blockchain" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 16, Name = "Realidad Aumentada" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 17, Name = "Robótica" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 18, Name = "Sistemas Embebidos" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 19, Name = "Ingeniería de Datos" , AcademicPeriod = AcademicPeriodType.Year},
                new Course { Id = 20, Name = "Desarrollo de Videojuegos" , AcademicPeriod = AcademicPeriodType.Year}
            };

            // Sample commissions

            commissionsSample = new List<Commission>
            {
                new Commission { Id = 1, Name = "Comisión A", Day = "Monday", Schedule = "8:00-10:00", CourseId = 1, ProfessorId = 2, Enabled = true,MaxEnrolls = 30 },
                new Commission { Id = 2, Name = "Comisión B", Day = "Friday", Schedule = "14:00-16:00", CourseId = 2,  ProfessorId = 2, Enabled = true, MaxEnrolls = 25 },
                new Commission { Id = 3, Name = "Comisión C", Day = "Tuesday", Schedule = "18:00-20:00", CourseId = 3,  ProfessorId = 4, Enabled = true, MaxEnrolls = 20 },
                new Commission { Id = 4, Name = "Comisión D", Day = "Wednesday", Schedule = "10:00-12:00", CourseId = 4,  ProfessorId = 2, Enabled = true, MaxEnrolls = 15 },
                new Commission { Id = 5, Name = "Comisión E", Day = "Thursday", Schedule = "16:00-18:00", CourseId = 5,  ProfessorId = 4, Enabled = true, MaxEnrolls = 10 },
                new Commission { Id = 6, Name = "Comisión F", Day = "Friday", Schedule = "20:00-22:00", CourseId = 6,  ProfessorId = 2, Enabled = true, MaxEnrolls = 5 },
                new Commission { Id = 7, Name = "Comisión G", Day = "Monday", Schedule = "8:00-10:00", CourseId = 7,  ProfessorId = 4, Enabled = true, MaxEnrolls = 30 },
                new Commission { Id = 8, Name = "Comisión H", Day = "Tuesday", Schedule = "14:00-16:00", CourseId = 8,  ProfessorId = 2, Enabled = true, MaxEnrolls = 25 },
                new Commission { Id = 9, Name = "Comisión I", Day = "Wednesday", Schedule = "18:00-20:00", CourseId = 9,  ProfessorId = 4, Enabled = true, MaxEnrolls = 20 },
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
