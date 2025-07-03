namespace InscripcionUniAPI.Core.Entities
{
    public class EnrolledCourse
    {
        public int Id { get; set; }

        // FKs
        public int SemesterEnrollmentId { get; set; }
        public int CourseId { get; set; }

        public int CreditHours { get; set; }

        // Propiedades de navegación
        public SemesterEnrollment? SemesterEnrollment { get; set; }
        public Course? Course { get; set; }
    }
}
