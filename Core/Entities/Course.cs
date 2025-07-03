namespace InscripcionUniAPI.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;    // Ejemplo: "MAT101"
        public string Title { get; set; } = string.Empty;   // Nombre del curso
        public int Credits { get; set; }                     // Créditos del curso

        // Relación con SemesterCourse
        public ICollection<SemesterCourse> SemesterCourses { get; set; } = new List<SemesterCourse>();
    }
}
