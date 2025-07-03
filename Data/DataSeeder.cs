using System.Collections.Generic;
using System.Linq;
using InscripcionUniAPI.Core.Entities;

namespace InscripcionUniAPI.Data
{
    public class DataSeeder
    {
        public void Seed()
        {
            // Simulación: obtienes lista de EnrolledCourse (de donde sea)
            List<EnrolledCourse> enrolledCourses = GetEnrolledCourses();

            // Convertimos List<EnrolledCourse> a ICollection<SemesterCourse>
            ICollection<SemesterCourse> semesterCourses = enrolledCourses
                .Select(ec => new SemesterCourse
                {
                    Id = ec.Id,
                    CourseId = ec.CourseId,
                    SemesterId = ec.SemesterId,
                    // Otras propiedades necesarias
                })
                .ToList();

            // Ejemplo de entidad a la que se asigna la colección de SemesterCourse
            var someEntity = new SomeEntity(); // Cambia SomeEntity por la entidad real

            someEntity.SemesterCourses = semesterCourses;

            // Ejemplo de agregar un solo SemesterCourse mapeado desde EnrolledCourse
            EnrolledCourse enrolledCourse = GetSingleEnrolledCourse();

            SemesterCourse semesterCourse = new SemesterCourse
            {
                Id = enrolledCourse.Id,
                CourseId = enrolledCourse.CourseId,
                SemesterId = enrolledCourse.SemesterId,
                // Otras propiedades necesarias
            };

            someEntity.SemesterCourses.Add(semesterCourse);

            // Aquí continuar con la lógica de si guardas en BD, etc.
        }

        // Métodos simulados para obtener datos
        private List<EnrolledCourse> GetEnrolledCourses()
        {
            // Aquí iría la lógica para obtener cursos inscritos
            return new List<EnrolledCourse>();
        }

        private EnrolledCourse GetSingleEnrolledCourse()
        {
            // Aquí iría la lógica para obtener un solo curso inscrito
            return new EnrolledCourse();
        }
    }

    // Ejemplo placeholder de entidad donde se asignan SemesterCourses
    public class SomeEntity
    {
        public ICollection<SemesterCourse> SemesterCourses { get; set; } = new List<SemesterCourse>();
    }
}
