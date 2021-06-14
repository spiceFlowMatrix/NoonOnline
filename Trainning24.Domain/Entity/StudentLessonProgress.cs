using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentLessonProgress : EntityBase, IStudentLessonProgress
    {
        public long LessonId { get; set; }
        public long LessonStatus { get; set; }
        public long StudentId { get; set; }
        public decimal LessonProgress { get; set; }
        public long Duration { get; set; }
    }
}
