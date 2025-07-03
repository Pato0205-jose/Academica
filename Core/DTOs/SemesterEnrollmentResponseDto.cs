<<<<<<< HEAD
namespace InscripcionUniAPI.Core.DTOs
=======
using System.Collections.Generic;

namespace InscripcionUniAPI.Core.Dtos
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
{
    public class SemesterEnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int Year { get; set; }
        public string Term { get; set; } = default!;
        public int MaxCreditHours { get; set; }

        public ICollection<EnrolledCourseDto> Courses { get; set; } = new List<EnrolledCourseDto>();
    }
<<<<<<< HEAD
=======

    public class EnrolledCourseDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int CreditHours { get; set; }
    }
>>>>>>> 8dfe60f3f6676b7b6824560c9e3969130bd59001
}
