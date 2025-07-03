namespace InscripcionUniAPI.Core.Entities
{
    public class SemesterCourse
    {
        public int Id { get; set; }

        // Llave foránea hacia Semester
        public int SemesterId { get; set; }
        public Semester Semester { get; set; } = null!;

        // Llave foránea hacia Course
        public int CourseId { get; set; }
        public Course Course { get; set; } = null!;

        // Puedes agregar propiedades adicionales como cupo, horario, etc.
    }
}

