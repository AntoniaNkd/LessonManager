using LessonManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LessonManager.ViewModel
{
    public class OverViewModel
    {
        public Lesson Lesson {  get; set; } = null;  
        public string UserRole {  get; set; }
        public TeachClass TeachClass { get; set; }


    }
}