namespace InscripcionUniAPI.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;    // Ejemplo: "MAT101"
        public string Name { get; set; } = string.Empty;    // Nombre del curso
        public int CreditHours { get; set; }                // Créditos del curso

        // Relación con EnrolledCourse
        public ICollection<EnrolledCourse> EnrolledCourses { get; set; } = new List<EnrolledCourse>();
    }
}
