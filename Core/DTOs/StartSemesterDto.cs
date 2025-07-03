using System.ComponentModel.DataAnnotations;

<<<<<<< HEAD
namespace InscripcionUniAPI.Core.DTOs
=======
namespace InscripcionUniAPI.Core.Dtos
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
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
<<<<<<< HEAD
} 
=======
}
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
