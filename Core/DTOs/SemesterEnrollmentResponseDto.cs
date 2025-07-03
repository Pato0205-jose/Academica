using System.Collections.Generic;

namespace InscripcionUniAPI.Core.Dtos
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

    public class EnrolledCourseDto
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Code { get; set; } = default!;
        public string Name { get; set; } = default!;
        public int CreditHours { get; set; }
    }
}
