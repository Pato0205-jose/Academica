using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Core.Helpers;
using InscripcionUniAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Services.Implementations
{
    public class StudentService : IStudentService
    {
        private readonly Data.UniversityDbContext _db;

        public StudentService(Data.UniversityDbContext db) => _db = db;

        public async Task<Student> CreateAsync(Student student)
        {
            if (await _db.Students.AnyAsync(s => s.Matriculation == student.Matriculation))
                throw new InvalidOperationException("Ya existe un estudiante con esa matrícula");
            if (await _db.Students.AnyAsync(s => s.Email == student.Email))
                throw new InvalidOperationException("Ya existe un estudiante con ese correo");

            _db.Students.Add(student);
            await _db.SaveChangesAsync();
            return student;
        }

        public async Task DeleteAsync(int id)
        {
            var student = await _db.Students.Include(s => s.SemesterEnrollments).FirstOrDefaultAsync(s => s.Id == id)
                          ?? throw new KeyNotFoundException("Estudiante no encontrado");

            if (student.SemesterEnrollments.Any())
                throw new InvalidOperationException("No se puede eliminar un estudiante con semestres inscritos");

            _db.Students.Remove(student);
            await _db.SaveChangesAsync();
        }

        public async Task<Student?> GetByIdAsync(int id) =>
            await _db.Students.FindAsync(id);

        public async Task<PaginatedList<Student>> GetPagedAsync(int page, int size) =>
            await PaginatedList<Student>.CreateAsync(_db.Students.OrderBy(s => s.Id), page, size);

        public async Task<Student> UpdateAsync(int id, Student student)
        {
            var existing = await _db.Students.FindAsync(id)
                           ?? throw new KeyNotFoundException("Estudiante no encontrado");

            existing.FirstName = student.FirstName;
            existing.LastName = student.LastName;
            await _db.SaveChangesAsync();
            return existing;
        }
    }
}
