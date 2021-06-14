using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentCourseProgress : EntityBase, IStudentCourseProgress
    {
        public long CourseId { get; set; }
        public long CourseStatus { get; set; }
        public decimal CourseProgress { get; set; }
        public long StudentId { get; set; }
    }
}
