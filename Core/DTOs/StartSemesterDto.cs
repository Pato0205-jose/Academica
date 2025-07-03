using System.ComponentModel.DataAnnotations;

namespace InscripcionUniAPI.Core.DTOs
{
    public class StartSemesterDto
    {
        [Required]
        public int Year { get; set; }

        [Required]
        [StringLength(20)]
        public string Term { get; set; } = default!;

        [Required]
        public int MaxCreditHours { get; set; }
    }
}
