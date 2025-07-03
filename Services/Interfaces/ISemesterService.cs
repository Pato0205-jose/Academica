using InscripcionUniAPI.Core.Dtos;
using System.Threading.Tasks;

namespace InscripcionUniAPI.Services.Interfaces
{
    /// <summary>
    /// Contrato de servicio para la gestión de inscripciones semestrales.
    /// Todos los métodos exponen/reciben DTOs simples para
    /// mantener la API limpia y evitar objetos anidados complejos.
    /// </summary>
    public interface ISemesterService
    {
        /// <summary>
        /// Crea un nuevo semestre para un estudiante.
        /// </summary>
        /// <param name="studentId">Id del estudiante al que se le crea el semestre.</param>
        /// <param name="dto">Datos básicos del semestre (año, término, etc.).</param>
        /// <returns>DTO con la información del semestre creado (sin bucles de navegación).</returns>
        Task<SemesterEnrollmentResponseDto> StartSemesterAsync(int studentId, StartSemesterDto dto);

        /// <summary>
        /// Obtiene un semestre por su Id.
        /// </summary>
        Task<SemesterEnrollmentResponseDto?> GetByIdAsync(int semesterId);

        /// <summary>
        /// Agrega un curso existente a un semestre.
        /// </summary>
        /// <param name="semesterId">Id del semestre.</param>
        /// <param name="courseId">Id del curso existente en la BD.</param>
        /// <returns>DTO actualizado del semestre con la lista de cursos.</returns>
        Task<SemesterEnrollmentResponseDto?> AddCourseAsync(int semesterId, int courseId);
    }
}
