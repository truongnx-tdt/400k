using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // Controllers/HomeController.cs
        public ActionResult Statistics()
        {
            var db = new StudentManagementContext();

            var stats = new StatisticsViewModel
            {
                TotalStudents = db.Students.Count(),
                TotalClasses = db.Classes.Count(),
                TotalSubjects = db.Subjects.Count(),
                TotalDepartments = db.Departments.Count(),

                SubjectAverages = db.AcademicRecords
                    .GroupBy(a => a.Subject.SubjectName)
                    .Select(g => new SubjectAverage
                    {
                        SubjectName = g.Key,
                        AverageScore = g.Average(a => a.TotalScore ?? 0)
                    })
                    .OrderByDescending(s => s.AverageScore)
                    .ToList(),

                StudentsByClass = db.Classes
                    .Select(c => new ClassStudentCount
                    {
                        ClassName = c.ClassName,
                        StudentCount = db.Students.Count(s => s.ClassId == c.ClassId)
                    })
                    .ToList()
            };

            ViewBag.TotalStudents = stats.TotalStudents;
            ViewBag.TotalClasses = stats.TotalClasses;
            ViewBag.TotalSubjects = stats.TotalSubjects;
            ViewBag.TotalDepartments = stats.TotalDepartments;

            return View(stats);
        }
    }
}