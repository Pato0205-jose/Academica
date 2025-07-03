using System.ComponentModel.DataAnnotations;

namespace InscripcionUniAPI.Core.Entities
{
    public class SemesterCourse
    {
        public int Id { get; set; }

        [Required]
        public int SemesterEnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int CreditHours { get; set; }

        // Propiedades de navegación
        public SemesterEnrollment? SemesterEnrollment { get; set; }
        public Course? Course { get; set; }
    }
}
