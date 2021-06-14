using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Abstract.Entity
{
    public interface IStudentCourseProgress : IEntityBase
    {
        long CourseId { get; set; }
        long CourseStatus { get; set; }
        decimal CourseProgress { get; set; }
        long StudentId { get; set; }
    }
}
