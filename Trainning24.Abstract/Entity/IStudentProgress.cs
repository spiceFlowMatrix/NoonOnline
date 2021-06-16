using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IStudentProgress : IEntityBase
    {
        long CourseId { get; set; }
        long ChapterId { get; set; }
        long LessonId { get; set; }
        long CourseStatus { get; set; }
        long ChapterStatus { get; set; }
        long LessonStatus { get; set; }
        decimal CourseProgress { get; set; }
        long StudentId { get; set; }
    }
}
