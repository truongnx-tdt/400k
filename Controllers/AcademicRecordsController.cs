using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StudentManagement.Models;

namespace StudentManagement.Controllers
{
    [AuthorizeRole("User", "Admin")]
    public class AcademicRecordsController : Controller
    {
        private StudentManagementContext db = new StudentManagementContext();

        // GET: AcademicRecords
        public ActionResult Index()
        {
            var academicRecords = db.AcademicRecords.Include(a => a.Class).Include(a => a.Student).Include(a => a.Subject);
            return View(academicRecords.ToList());
        }

        // GET: AcademicRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicRecord academicRecord = db.AcademicRecords.Find(id);
            if (academicRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.recordId = id;
            return View(academicRecord);
        }

        // GET: AcademicRecords/Create
        public ActionResult Create()
        {
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName");
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode");
            return View();
        }

        // POST: AcademicRecords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RecordId,StudentId,SubjectId,ClassId,Semester,Year,MidtermScore,FinalScore,ComponentScore,TotalScore,LetterGrade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                db.AcademicRecords.Add(academicRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", academicRecord.ClassId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName", academicRecord.StudentId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode", academicRecord.SubjectId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicRecord academicRecord = db.AcademicRecords.Find(id);
            if (academicRecord == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", academicRecord.ClassId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName", academicRecord.StudentId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode", academicRecord.SubjectId);
            return View(academicRecord);
        }

        // POST: AcademicRecords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RecordId,StudentId,SubjectId,ClassId,Semester,Year,MidtermScore,FinalScore,ComponentScore,TotalScore,LetterGrade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                var existingRecord = db.AcademicRecords.Find(academicRecord.RecordId);
                if (existingRecord == null)
                {
                    return HttpNotFound();
                }

                db.Entry(existingRecord).CurrentValues.SetValues(academicRecord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", academicRecord.ClassId);
            ViewBag.StudentId = new SelectList(db.Students, "StudentId", "FullName", academicRecord.StudentId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode", academicRecord.SubjectId);
            return View(academicRecord);
        }

        // GET: AcademicRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicRecord academicRecord = db.AcademicRecords.Find(id);
            if (academicRecord == null)
            {
                return HttpNotFound();
            }
            return View(academicRecord);
        }

        // POST: AcademicRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AcademicRecord academicRecord = db.AcademicRecords.Find(id);
            db.AcademicRecords.Remove(academicRecord);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult StudentGrades(int studentId)
        {
            var student = db.Students.Find(studentId);
            if (student == null)
            {
                return HttpNotFound();
            }

            var academicRecords = db.AcademicRecords
                .Include(a => a.Class)
                .Include(a => a.Student)
                .Include(a => a.Subject)
                .Where(a => a.StudentId == studentId)
                .ToList();
            ViewBag.StudentId = studentId;
            ViewBag.StudentName = student.FullName;
            return View("Index", academicRecords);
        }
        public ActionResult AddResult(int studentId)
        {
            var student = db.Students.Find(studentId);
            if (student == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.StudentId = studentId;
            ViewBag.StudentName = student.FullName;
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName");
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode");
            return View(new AcademicRecord { StudentId = studentId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddResult([Bind(Include = "RecordId,StudentId,SubjectId,ClassId,Semester,Year,MidtermScore,FinalScore,ComponentScore,TotalScore,LetterGrade")] AcademicRecord academicRecord)
        {
            if (ModelState.IsValid)
            {
                db.AcademicRecords.Add(academicRecord);
                db.SaveChanges();
                return RedirectToAction("StudentGrades", new { studentId = academicRecord.StudentId });
            }
            var student = db.Students.Find(academicRecord.StudentId);
            academicRecord.StudentId = student.StudentId;
            ViewBag.StudentId = academicRecord.StudentId;
            ViewBag.StudentName = student?.FullName;
            ViewBag.ClassId = new SelectList(db.Classes, "ClassId", "ClassName", academicRecord.ClassId);
            ViewBag.SubjectId = new SelectList(db.Subjects, "SubjectId", "SubjectCode", academicRecord.SubjectId);
            return View(academicRecord);
        }

    }
}
