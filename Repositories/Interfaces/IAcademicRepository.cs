using Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Interfaces
{
    public interface IAcademicRepository
    {
        List<Specialty> GetSpecialties();
        List<Course> GetCourses();
        List<CurricularPlan> GetCurricularPlans();
        List<Commission> GetCommissions();
        List<Enrollment> GetEnrollments();
        List<Grade> GetGrades();
        void AddSpecialty(Specialty specialty);
        void AddCourse(Course course);
        void AddCurricularPlan(CurricularPlan curricularPlan);
        void AddCommission(Commission commission);
        void AddEnrollment(Enrollment enrollment);
        void AddGrade(Grade grade);
        void UpdateSpecialty(Specialty specialty);
        void UpdateCourse(Course course);
        void UpdateCurricularPlan(CurricularPlan curricularPlan);
        void UpdateCommission(Commission commission);
        void UpdateEnrollment(Enrollment enrollment);
        void UpdateGrade(Grade grade);
        void DeleteSpecialty(int id);
        void DeleteCourse(int id);
        void DeleteCurricularPlan(int id);
        void DeleteCommission(int id);
        void DeleteEnrollment(int id);
        void DeleteGrade(int id);
        Specialty GetSpecialtyById(int id);
        Course GetCourseById(int id);
        CurricularPlan GetCurricularPlanById(int id);
        Commission GetCommissionById(int id);
        Enrollment GetEnrollmentById(int id);
        Grade GetGradeById(int id);
        List<Specialty> GetSpecialtiesByCourseId(int courseId);
        List<Commission> GetCommissionsByCourseId(int courseId);
        List<Enrollment> GetEnrollmentsByStudentId(int studentId);
        List<Grade> GetGradesByStudentId(int studentId);
        List<Commission> GetCommissionsByProfessorId(int professorId);
        List<Course> GetCoursesBySpecialtyId(int specialtyId);
        List<CurricularPlan> GetCurricularPlansBySpecialtyId(int specialtyId);
        List<Commission> GetCommissionsByCurricularPlanId(int curricularPlanId);
        List<Enrollment> GetEnrollmentsByCommissionId(int commissionId);
        List<Grade> GetGradesByCommissionId(int commissionId);
        List<Commission> GetCommissionsByStudentId(int studentId);
        List<Commission> GetCommissionsByCourseAndProfessorId(int courseId, int professorId);
        List<Commission> GetCommissionsByCourseAndDay(int courseId, string day);
        List<Commission> GetCommissionsByCourseAndShift(int courseId, string shift);
        List<Commission> GetCommissionsByCourseAndSchedule(int courseId, string schedule);
        List<Commission> GetCommissionsByCourseAndProfessorIdAndDay(int courseId, int professorId, string day);
        List<Commission> GetCommissionsByCourseAndProfessorIdAndShift(int courseId, int professorId, string shift);
        List<Commission> GetCommissionsByCourseAndProfessorIdAndSchedule(int courseId, int professorId, string schedule);
        List<Commission> GetCommissionsByCourseAndDayAndShift(int courseId, string day, string shift);
        List<Commission> GetCommissionsByCourseAndDayAndSchedule(int courseId, string day, string schedule);


    }
}
