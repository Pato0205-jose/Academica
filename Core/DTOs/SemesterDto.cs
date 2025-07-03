namespace InscripcionUniAPI.Core.Dtos
{
    public class StartSemesterDto
    {
        public int Year { get; set; }
        public string Term { get; set; } = string.Empty;
        public int MaxCreditHours { get; set; }
    }
}
