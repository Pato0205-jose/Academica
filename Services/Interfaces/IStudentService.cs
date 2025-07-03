using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Core.Helpers;

namespace InscripcionUniAPI.Services.Interfaces
{
    public interface IStudentService
    {
        Task<Student> CreateAsync(Student student);
        Task<PaginatedList<Student>> GetPagedAsync(int page, int size);
        Task<Student?> GetByIdAsync(int id);
        Task<Student> UpdateAsync(int id, Student student);
        Task DeleteAsync(int id);
    }
}
