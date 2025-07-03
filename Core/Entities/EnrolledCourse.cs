namespace InscripcionUniAPI.Core.Entities
{
    public class EnrolledCourse
    {
        public int Id { get; set; }
        public int SemesterEnrollmentId { get; set; }
        public int CourseId { get; set; }
        public byte CreditHours { get; set; }

        public SemesterEnrollment? SemesterEnrollment { get; set; }
        public Course? Course { get; set; }
    }
}
