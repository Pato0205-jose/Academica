using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Core.Helpers;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Services.Implementations
{
    public class CourseService : ICourseService
    {
        private readonly Data.UniversityDbContext _db;

        public CourseService(Data.UniversityDbContext db) => _db = db;

        public async Task<Course> CreateAsync(Course course)
        {
            if (await _db.Courses.AnyAsync(c => c.Code == course.Code))
                throw new InvalidOperationException("Ya existe un curso con ese código");

            _db.Courses.Add(course);
            await _db.SaveChangesAsync();
            return course;
        }

        public async Task<PaginatedList<Course>> GetPagedAsync(int page, int size) =>
            await PaginatedList<Course>.CreateAsync(_db.Courses.OrderBy(c => c.Id), page, size);
    }
}
