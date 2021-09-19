using System.Collections.Generic;

namespace LessonManager.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Lesson> Reqiured { get; set; }

        public string Status { get; set; }
    }
}