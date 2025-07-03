using System.ComponentModel.DataAnnotations;

namespace InscripcionUniAPI.Core.Entities
{
<<<<<<< HEAD
    public class SemesterEnrollment
=======
    public class SemesterCourse
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
    {
        public int Id { get; set; }

        [Required]
<<<<<<< HEAD
        public int StudentId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required, StringLength(20)]
        public string Term { get; set; } = default!;

        [Required]
        public int MaxCreditHours { get; set; }

        // Navegación
        public Student? Student { get; set; }

        public ICollection<EnrolledCourse> Courses { get; set; } = new List<EnrolledCourse>();
=======
        public int SemesterEnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int CreditHours { get; set; }

        // Propiedades de navegación
        public SemesterEnrollment? SemesterEnrollment { get; set; }
        public Course? Course { get; set; }
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
    }
}
