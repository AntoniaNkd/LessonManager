namespace LessonManager.Models
{
    public class TeachClass
    {
        public int Id { get; set; }
        public Lesson Lesson { get; set; }
        public ApplicationUser Teacher { get; set; }
        public int Year { get; set; }
        public int Semester { get; set; }
        public float ExamWeight { get; set; }
        public float LabWeight { get; set; }
        public bool ExamMandatory { get; set; }
        public bool LabMandatory { get; set; }
    }
}