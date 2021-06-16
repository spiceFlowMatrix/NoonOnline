using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;


namespace Trainning24.Domain.Entity
{
    public class StudentLessonComplete //: EntityBase, IStudentLessonComplete
    {
        public Lesson Lesson { get; set; }
        //public Video Video { get; set; }
        public PdfFile PdfFile { get; set; }
        public Quiz Quiz { get; set; }
        public StudentLessonProgress StudentProgress { get; set; }
    }
}
