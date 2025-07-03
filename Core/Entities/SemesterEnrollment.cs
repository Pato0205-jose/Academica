using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using InscripcionUniAPI.Core.Exceptions;

namespace InscripcionUniAPI.Core.Entities
{
    [Index(nameof(StudentId), nameof(Year), nameof(Term), IsUnique = true)]
    public class SemesterEnrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public ushort Year { get; set; }
        [Required]
        public string Term { get; set; } = default!;
        public byte MaxCreditHours { get; set; } = 21;

        public List<EnrolledCourse> Courses { get; set; } = new();

        public void AddCourse(Course course)
        {
            var total = Courses.Sum(c => c.CreditHours) + course.CreditHours;
            if (total > MaxCreditHours)
                throw new BusinessRuleViolationException($"Se excede el límite de {MaxCreditHours} créditos.");

            Courses.Add(new EnrolledCourse
            {
                CourseId = course.Id,
                CreditHours = course.CreditHours
            });
        }
    }
}
