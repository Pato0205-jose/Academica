using InscripcionUniAPI.Core.Entities;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterEnrollment> StartSemesterAsync(int studentId, SemesterEnrollment semester);
        Task<SemesterEnrollment?> GetByIdAsync(int semesterId);
        Task<SemesterEnrollment?> AddCourseAsync(int semesterId, int courseId); // <- esta línea es la que debes corregir
    }
}
