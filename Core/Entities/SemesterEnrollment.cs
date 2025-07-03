using System.Collections.Generic;

namespace InscripcionUniAPI.Core.Entities
{
    public class SemesterEnrollment
    {
        public int Id { get; set; }

        // FK al estudiante
        public int StudentId { get; set; }

        public ushort Year { get; set; }          // 2023, 2024, 2025…
        public string Term { get; set; } = null!; // "Primavera", "Otoño", etc.
        public byte MaxCreditHours { get; set; } = 21;

        // Propiedades de navegación
        public Student? Student { get; set; }
        public List<EnrolledCourse> Courses { get; set; } = new();
    }
}
