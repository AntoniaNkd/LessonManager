using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using LessonManager.Models;
using Microsoft.AspNet.Identity;
using AppContext = LessonManager.Models.AppContext;
using LessonManager.ViewModel;

namespace LessonManager.Controllers
{
    public class LessonsController : Controller
    {
        // GET: Lessons
        public ActionResult Index()
        {
            return View(new OverViewModel() { UserRole = this.GetUserRole().Role});
        }

        public ActionResult Marks()
        {
            return View(new OverViewModel() { UserRole = this.GetUserRole().Role }); ;
        }

        public ApplicationUser GetUserRole()
        {
            var uid = User.Identity.GetUserId();
            using (var db = new Models.AppContext())
            {
                return db.ApplicationUsers.FirstOrDefault(sh => sh.Id == uid);
            }
        }

        public ActionResult OverView(int Id)
        {
            Lesson found = null;
            // Φόρτωση του μαθήματος με βάση το id του μαθήματος που επιλέχθηκε
            using (var db = new AppContext())
            {
                found = db.Lessons.First(sh => sh.Id == Id);
            }
            TeachClass teachclass = null;
            using (var db = new Models.AppContext()) { 
                teachclass = db.Classes.FirstOrDefault(sh => sh.Lesson.Id == found.Id);
            }
            return View(new OverViewModel() {Lesson= found, UserRole = this.GetUserRole().Role , TeachClass = teachclass});
        }

        public ActionResult GetClasses()
        {
            var user = this.GetUserRole();
            switch (user.Role) { 
                case "student":
                    using(var db = new Models.AppContext()) {
                        List<DilosiViewModel> dilosis = new List<DilosiViewModel>();
                        var classes = db.Classes.Include(sh=>sh.Teacher).Include(sh=>sh.Lesson).ToList();
                        foreach(var cl in classes) {
                            var dil = db.Dilosis
                                .Include(sh => sh.TeachClass)
                                .Include(sh => sh.User)
                                .Where(sh => sh.TeachClass.Id == cl.Id && sh.User.Id == user.Id)
                                .FirstOrDefault();
                            dilosis.Add(new DilosiViewModel()
                            {
                                Id = cl.Id,
                                LessonName = cl.Lesson.Name,
                                TeacherName = cl.Teacher.FullName,
                                Semester = cl.Semester,
                                LabMark = (dil != null)? dil.LabMark : 0,
                                ExamMark = (dil != null) ? dil.ExamMark : 0,
                                FinalMark = (dil!=null) ? this.CalculataFinalMark(dil.Id, dil.LabMark, dil.ExamMark) : 0,
                                Status = (dil !=null) ? "Βαθμολογήθηκε" : "Δεν έχει βαθμολογηθεί"

                            });
                        }
                        return Json(new { data = dilosis }, JsonRequestBehavior.AllowGet);
                    }
                    break;
                case "teacher":
                    using (var db = new Models.AppContext())
                    {
                        var teachClassesIds = db.Classes
                            .Include(sh => sh.Teacher)
                            .Include(sh => sh.Lesson)
                            .Where(sh => sh.Teacher.Id == user.Id).ToList()
                            .Select(sh => sh.Id);
                        List<DilosiViewModel> dilosis = new List<DilosiViewModel>();
                        var classes = db.Dilosis
                            .Include(sh => sh.TeachClass)
                            .Include(sh => sh.User)
                            .Where(sh => teachClassesIds.Contains(sh.TeachClass.Id) ).ToList();

                        foreach (var cl in classes)
                        {
                            dilosis.Add(new DilosiViewModel()
                            {
                                Id = cl.Id,
                                LessonName = cl.TeachClass.Lesson.Name,
                                TeacherName = cl.User.FullName,
                                Semester = cl.TeachClass.Semester,
                                LabMark = cl.LabMark,
                                ExamMark = cl.ExamMark,
                                FinalMark =(cl.LabMark != 0) ? this.CalculataFinalMark(cl.Id, cl.LabMark, cl.ExamMark) : 0,
                                Status = (cl.LabMark != 0) ? "Βαθμολογήθηκε" :"Δεν έχει βαθμολογηθεί"

                            });
                        }
                        return Json(new { data = dilosis }, JsonRequestBehavior.AllowGet);
                    }
            }
            return Json(new { data = "fail" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddLesson(string name, string description, int id = 0)
        {
            var _toAdd = new Lesson {Name = name, Description = description, Status= "free"};
            if (id != 0) { _toAdd.Id = id; _toAdd.Status = "taken"; }
            try
            {
                using (var db = new AppContext())
                {
                    db.Lessons.AddOrUpdate(_toAdd);
                    db.SaveChanges();
                    return Json(new {status = "success"});
                    ;
                }
            }
            catch (Exception ex)
            {
                return Json(new {status = "success", message = ex.ToString()});
                ;
            }
        }

        public JsonResult ManageDilosi(int id, int status)
        {
            switch (status)
            {
                // Αφαίρεση απο την δήλωση 
                case 0 :
                    using(var db = new AppContext())
                    {
                        Dilosi d =db.Dilosis.First(sh => sh.TeachClass.Id == id);
                        db.Dilosis.Remove(d);
                        db.SaveChanges();
                    }
                    break;
                case 1 :
                    Random rnd = new Random();
                    using (var db = new AppContext())
                    {
                        var uid = User.Identity.GetUserId();
                        ApplicationUser user = db.ApplicationUsers.First(sh => sh.Id == uid);
                        Dilosi d = new Dilosi()
                        {   Id = rnd.Next(10000000), 
                            TeachClass = db.Classes.First(sh => sh.Id == id),
                            User = user,  
                        };
                        db.Dilosis.Add(d);
                        db.SaveChanges();
                    }
                    break;

            }
            return Json(new {status = "success"});     
        }

        [HttpPost]
        public JsonResult EditClass(
            int id ,
            float examWeight ,
            float labWeight ,
            bool labMandatory, 
            bool examMandatory)
        {
            try
            {
                // Φορτώνουμε την τάξη του Διδάσκωντα και ανανεώνουμε τις πληροφορίες 
                using(var db = new AppContext())
                {
                    var teachClass = db.Classes.FirstOrDefault(sh => sh.Id == id); 
                    teachClass.ExamMandatory= examMandatory;
                    teachClass.LabWeight = labWeight;
                    teachClass.LabMandatory= labMandatory;      
                    teachClass.ExamWeight= examWeight;
                    db.Classes.AddOrUpdate(teachClass);
                    db.SaveChanges();
                    return Json(new { status = "success" });
                } 
            }catch (Exception ex) {
                return Json(new { status = "fail" });
            }  
        }

        public JsonResult makeClass(string teacher, string semester, int lesson_id)
        {
            // Ανάθεση Μαθήματος 
            // Αρχικά πρέπει να φορτώσουμε τον καθήτή που αφορά το μάθημα 
            // το ίδιο το μάθημα καθώς και να κατακσευάσουμε μια τάξη 
            using(var db = new AppContext())
            {
                // Φόρτωση του διδάσκοντα και του επιλεγμένου μαθήματος
                ApplicationUser applicationUser = db.ApplicationUsers.FirstOrDefault(sh => sh.Id == teacher);
                Lesson lesson = db.Lessons.FirstOrDefault(sh => sh.Id == lesson_id);
                if(lesson != null && applicationUser !=null)
                {
                    TeachClass teachClass= new TeachClass() { Lesson = lesson, Teacher = applicationUser, Year = DateTime.Now.Year, Semester = int.Parse(semester) };   
                    db.Classes.Add(teachClass);
                    lesson.Status = "taken";
                    db.Lessons.AddOrUpdate(lesson);
                    db.SaveChanges();
                    return Json(new { status = "success" });
                }   
            }
            return Json(new { status = "error" });            
        }

        [HttpPost]
        public JsonResult DeleteLesson(int id)
        {
            // Εύρεση του μαθήματος που πρόκειται να διαγαρφεί
            using (var db = new AppContext())
            {
                var lesson = db.Lessons.FirstOrDefault(sh => sh.Id == id);
                db.Lessons.Remove(lesson);
                db.SaveChanges();
            }

            return Json(new {status = "success"});
            ;
        }

        /**
         * Επιστρέφει όλα τα μαθήματα που έχουν προστεθέι απο τον χρήστη
         */
        [HttpGet]
        public JsonResult GetAllLessons()
        {
            // Φιλτράρουμε τα μαθήματα με βάση το ρόλο του κάθε χρήστη
            // π.χ. στον καθήγητη εμφανίζονται μόνο τα δικά του μαθήματα 
            string Uid = User.Identity.GetUserId();

            try
            {
                using (var db = new AppContext())
                {
                    db.Configuration.LazyLoadingEnabled = true;
                    var user = db.ApplicationUsers.FirstOrDefault(sh => sh.Id == Uid);
                    List<Lesson> lessons = new List<Lesson>();
                    if (user != null)
                    {
                        switch (user.Role)
                        {
                            case "teacher":
                                var items = db.Classes
                                     .Include(sh => sh.Lesson)
                                     .Where(sh => sh.Teacher.Id == user.Id)
                                     .ToList()
                                     .Select(sh => sh.Lesson);
                                return Json(new { data = items }, JsonRequestBehavior.AllowGet);
                            case "student":
                                items = db.Dilosis
                                   .Include(sh => sh.TeachClass)
                                   .Where(sh => sh.User.Id == user.Id)
                                   .Select(sh => sh.TeachClass)
                                   .Include(sh => sh.Lesson)
                                   .ToList()
                                   .Select(sh => sh.Lesson);
                                return Json(new { data = items }, JsonRequestBehavior.AllowGet);
                            default:
                                lessons = db.Lessons.ToList();
                                break;
                        }
                    } else
                    {
                        lessons = db.Lessons.ToList();
                        Console.WriteLine(lessons);
                    }
                  
                    return Json(new { data = lessons }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new {status = ex.ToString()}, JsonRequestBehavior.AllowGet);
            }
        }

        public float CalculataFinalMark(int TeachClassId, float labmark, float exammmark)
        {
            float finalMark = 0;
            // Αρχικα απο την δήλωση λαμβάνουμε την Τάξη και κατα συνέπεις τους βαθμούς θεωρίας και εργαστηρίου
            using (var db = new AppContext())
            {
                var currentClass = db.Dilosis
                    .Include(sh => sh.TeachClass)
                    .First(sh => sh.Id == TeachClassId);
                currentClass.LabMark = labmark;
                currentClass.ExamMark = exammmark;
                db.Dilosis.AddOrUpdate(currentClass);
                db.SaveChanges();

                finalMark = (currentClass.TeachClass.LabWeight * labmark)
                    + (currentClass.TeachClass.ExamWeight * exammmark);
      
            }
            return (float)Math.Round((float)finalMark, 2); ;

        }
    }
}