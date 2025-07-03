namespace InscripcionUniAPI.Core.Entities
{
    public class Semester
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // ejemplo: "2025-1"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Relación con cursos del semestre
        public ICollection<SemesterCourse> SemesterCourses { get; set; } = new List<SemesterCourse>();
    }
}
