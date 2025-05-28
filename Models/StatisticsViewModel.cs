// Models/StatisticsViewModel.cs
using System.Collections.Generic;

namespace StudentManagement.Models
{
    public class StatisticsViewModel
    {
        public int TotalStudents { get; set; }
        public int TotalClasses { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalDepartments { get; set; }

        public List<SubjectAverage> SubjectAverages { get; set; }
        public List<ClassStudentCount> StudentsByClass { get; set; }
    }

    public class SubjectAverage
    {
        public string SubjectName { get; set; }
        public double AverageScore { get; set; }
    }

    public class ClassStudentCount
    {
        public string ClassName { get; set; }
        public int StudentCount { get; set; }
    }
}