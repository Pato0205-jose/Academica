using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
        public ICollection<EnrolledCourse> Courses { get; set; } = new List<EnrolledCourse>();

        // Métodos de dominio
        public bool CanAddCourse(int newCourseCreditHours)
        {
            var currentTotalCredits = Courses.Sum(c => c.CreditHours);
            return (currentTotalCredits + newCourseCreditHours) <= MaxCreditHours;
        }

        public int GetCurrentCreditHours()
        {
            return Courses.Sum(c => c.CreditHours);
        }

        public int GetRemainingCreditHours()
        {
            return MaxCreditHours - GetCurrentCreditHours();
        }
    }
}
