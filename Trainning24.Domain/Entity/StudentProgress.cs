using System;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentProgress : EntityBase, IStudentProgress
    {
        public long CourseId { get; set; }
        public long ChapterId { get; set; }
        public long LessonId { get; set; }
        public long CourseStatus { get; set; }
        public long ChapterStatus { get; set; }
        public long LessonStatus { get; set; }
        public decimal CourseProgress { get; set; }
        public long StudentId { get; set; }
    }
}
