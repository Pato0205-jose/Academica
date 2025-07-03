using System.Threading.Tasks;
using InscripcionUniAPI.Core.Entities;
using InscripcionUniAPI.Data;

namespace InscripcionUniAPI.Services.Implementations
{
    public class SemesterService
    {
        private readonly UniversityDbContext _context;

        public SemesterService(UniversityDbContext context)
        {
            _context = context;
        }

        public async Task<Semester?> GetSemesterAsync(int id)
        {
            var semester = await _context.Semesters.FindAsync(id);
            if (semester == null)
                return null;

            // Acceso seguro a colección para evitar warnings
            var coursesCount = semester.SemesterCourses?.Count ?? 0;

            if (semester.SemesterCourses != null)
            {
                foreach (var course in semester.SemesterCourses)
                {
                    // Procesar course de forma segura
                }
            }

            return semester;
        }

        // Otros métodos del servicio...
    }
}
