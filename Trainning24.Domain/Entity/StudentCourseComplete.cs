using System;
using System.Collections.Generic;
using Trainning24.Abstract.Entity;


namespace Trainning24.Domain.Entity
{
    public class StudentCourseComplete //: EntityBase, IBundleCourseComplete
    {
		public Course Course { get; set; }
		public List<StudentChapterComplete> Chapters { get; set; }
		public StudentCourseProgress StudentProgress { get; set; }
    }
}
