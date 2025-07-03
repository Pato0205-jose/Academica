using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Core.Entities
{
    [Index(nameof(Matriculation), IsUnique = true)]
    [Index(nameof(Email), IsUnique = true)]
    public class Student
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string Matriculation { get; set; } = default!;

        [Required, StringLength(60)]
        public string FirstName { get; set; } = default!;

        [Required, StringLength(60)]
        public string LastName { get; set; } = default!;

        [Required, StringLength(100)]
        public string Email { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<SemesterEnrollment> SemesterEnrollments { get; set; } = new();
    }
}
