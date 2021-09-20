using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonManager.ViewModel
{
    public class DilosiViewModel
    {
        public int Id { get; set; }
        public string LessonName { get; set; }
        public string TeacherName { get; set; }
        public int Semester { get; set;  }
        public float FinalMark { get; set; }
        public float ExamMark { get; set; }
        public float LabMark { get; set; }
        public string Status { get; set; }


    }
}