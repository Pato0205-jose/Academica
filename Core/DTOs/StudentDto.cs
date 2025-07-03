namespace InscripcionUniAPI.Core.DTOs
{
    public class StudentDto
    {
        public string Matriculation { get; set; } = default!;
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
