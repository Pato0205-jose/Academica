namespace InscripcionUniAPI.Dtos
{
    public class SemesterEnrollmentDto
    {
        public int Year { get; set; }
        public string Term { get; set; } = string.Empty;
        public int MaxCreditHours { get; set; }
    }
}
