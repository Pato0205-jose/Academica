using InscripcionUniAPI.Core.DTOs;
using InscripcionUniAPI.Core.Entities;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Services.Interfaces
{
    public interface ISemesterService
    {
        Task<SemesterEnrollmentResponseDto> StartSemesterAsync(int studentId, StartSemesterDto dto);
        Task<SemesterEnrollmentResponseDto?> GetByIdAsync(int semesterId);
        Task<SemesterEnrollmentResponseDto?> AddCourseAsync(int semesterId, int courseId);
    }
}
