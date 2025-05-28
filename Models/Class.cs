using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class Class
    {
        [Key]
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public int DepartmentId { get; set; }
        public int AcademicYear { get; set; }
        public string HomeroomTeacher { get; set; }
        public virtual Department Department { get; set; }
    }
}