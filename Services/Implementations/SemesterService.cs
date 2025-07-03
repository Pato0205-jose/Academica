using System;
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

        public async Task<Semester> GetSemesterAsync(int id)
        {
            // Evitar posible null reference usando operador ? 
            var semester = await _context.Semesters.FindAsync(id);

            if (semester == null)
            {
                // Manejo si no se encuentra semestre
                return null!;
            }

            // Ejemplo uso seguro de referencia (línea 69 aprox)
            var coursesCount = semester.SemesterCourses?.Count ?? 0;

            // Otros usos seguros (línea 124 aprox)
            if (semester.SemesterCourses != null)
            {
                foreach (var course in semester.SemesterCourses)
                {
                    // trabajo seguro con course
                }
            }

            return semester;
        }

        // Otros métodos del servicio...
    }
}
