using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class AcademicRecord
    {
        [Key]
        public int RecordId { get; set; }
        public int StudentId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
        public int Semester { get; set; }
        public int Year { get; set; }

        [Column(TypeName = "float")]
        public float? MidtermScore { get; set; }

        [Column(TypeName = "float")]
        public float? FinalScore { get; set; }

        [Column(TypeName = "float")]
        public float? ComponentScore { get; set; }

        [Column(TypeName = "float")]
        public float? TotalScore { get; set; }

        public string LetterGrade { get; set; }

        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Class Class { get; set; }
    }
}