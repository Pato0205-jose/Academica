namespace InscripcionUniAPI.Dtos
{
    public class SemesterEnrollmentResponseDto
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int Year { get; set; }
        public string Term { get; set; } = string.Empty;
        public int MaxCreditHours { get; set; }
        public List<CourseDto> Courses { get; set; } = new();
    }

    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CreditHours { get; set; }
    }
}
