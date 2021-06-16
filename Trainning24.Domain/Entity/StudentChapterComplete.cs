using System;
using System.Collections.Generic;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class StudentChapterComplete //: EntityBase, IStudentChapterComplete
    {
		public Chapter Chapter { get; set;}
		public List<StudentLessonComplete> Lessons { get; set; }
		public StudentChapterProgress StudentProgress { get; set; }
    }
}
