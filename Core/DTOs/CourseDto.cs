namespace InscripcionUniAPI.Core.DTOs
{
    public class CourseDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CreditHours { get; set; }
    }
} 