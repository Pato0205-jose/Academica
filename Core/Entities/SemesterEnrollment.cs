using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace InscripcionUniAPI.Core.Entities
{
    public class SemesterEnrollment
    {
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required, StringLength(20)]
        public string Term { get; set; } = default!;

        [Required]
        public int MaxCreditHours { get; set; }

        // Navegación
        public Student? Student { get; set; }

        public ICollection<SemesterEnrollmentCourse> Courses { get; set; } = new List<SemesterEnrollmentCourse>();
    }

    public class SemesterEnrollmentCourse
    {
        public int Id { get; set; }

        [Required]
        public int SemesterEnrollmentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required]
        public int CreditHours { get; set; }

        // Navegación
        public SemesterEnrollment? SemesterEnrollment { get; set; }

        public Course? Course { get; set; }
    }
}
