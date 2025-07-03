using InscripcionUniAPI.Core.Entities;

namespace InscripcionUniAPI.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterEnrollment> StartSemesterAsync(int studentId, SemesterEnrollment semester);
        Task<EnrolledCourse> AddCourseAsync(int semesterId, int courseId);
        Task<SemesterEnrollment?> GetByIdAsync(int id);
    }
}
