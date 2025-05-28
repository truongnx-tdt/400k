using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    public class StudentsController : Controller
    {
        private StudentManagementContext db = new StudentManagementContext();

        // GET: Students
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Class);
            return View(students.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StudentId,FullName,Email,PhoneNumber,Address,DateOfBirth,Gender,ImagePath,ClassId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", student.ClassId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", student.ClassId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StudentId,FullName,Email,PhoneNumber,Address,DateOfBirth,Gender,ImagePath,ClassId")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", student.ClassId);
            return View(student);
        }
        [AuthorizeRole("Admin")]
        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        [AuthorizeRole("Admin")]
        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                if (student == null)
                {
                    return HttpNotFound();
                }

                db.Students.Remove(student);
                db.SaveChanges();

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }
                throw;
            }
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Students/AcademicRecords/5
        public ActionResult AcademicRecords(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var academicRecords = db.AcademicRecords.Include(a => a.Subject).Where(a => a.StudentId == id).ToList();
            if (academicRecords == null || !academicRecords.Any())
            {
                return HttpNotFound();
            }
            return View("Index", academicRecords);
        }

        // GET: Students/GetDetails/5
        public JsonResult GetDetails(int id)
        {
            var student = db.Students
                .Include(s => s.Class)
                .FirstOrDefault(s => s.StudentId == id);

            if (student == null)
            {
                return Json(new { error = "Student not found" }, JsonRequestBehavior.AllowGet);
            }

            var studentDetails = new
            {
                fullName = student.FullName,
                className = student.Class?.ClassName,
                email = student.Email,
                phoneNumber = student.PhoneNumber,
                address = student.Address,
                dateOfBirth = student.DateOfBirth,
                gender = student.Gender
            };

            return Json(JsonConvert.SerializeObject(studentDetails), JsonRequestBehavior.AllowGet);
        }
    }
}
