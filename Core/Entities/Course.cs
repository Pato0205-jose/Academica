using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace InscripcionUniAPI.Core.Entities
{
    [Index(nameof(Code), IsUnique = true)]
    public class Course
    {
        public int Id { get; set; }

        [Required, StringLength(10)]
        public string Code { get; set; } = default!;

        [Required, StringLength(120)]
        public string Name { get; set; } = default!;

        [Range(1, 9)]
        public byte CreditHours { get; set; }
    }
}
