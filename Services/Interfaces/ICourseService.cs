using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Core.Helpers;

namespace InscripcionUniAPI.Services.Interfaces
{
    public interface ICourseService
    {
        Task<Course> CreateAsync(Course course);
        Task<PaginatedList<Course>> GetPagedAsync(int page, int size);
    }
}
