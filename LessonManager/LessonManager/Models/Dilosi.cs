using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace LessonManager.Models
{
    public class Dilosi
    {
        public int Id { get; set; }
        public TeachClass TeachClass {  get; set; } 
        public ApplicationUser User { get; set;  }
        public float ExamMark  {  get; set; }
        public float LabMark {  get; set; }

    }
}