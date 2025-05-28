using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentManagement.Models
{
    public class Subject
    {
        [Key]
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; }
        public string SubjectName { get; set; }
        public int Credit { get; set; }
        public int DepartmentId { get; set; }
        public string Description { get; set; }
        public virtual Department Department { get; set; }
    }
}