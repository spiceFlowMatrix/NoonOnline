using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IStudentLessonProgress : IEntityBase
    {
        long LessonId { get; set; }
        long LessonStatus { get; set; }
        long StudentId { get; set; }
        decimal LessonProgress { get; set; }
        long Duration { get; set; }
    }
}
