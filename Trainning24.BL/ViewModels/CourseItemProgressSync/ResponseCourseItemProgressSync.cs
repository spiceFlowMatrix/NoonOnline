using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.CourseItemProgressSync
{
    public class ResponseCourseItemProgressSync
    {
        public long id { get; set; }
        public long userid { get; set; }
        public long lessonid { get; set; }
        public long lessonprogress { get; set; }
        public long quizid { get; set; }
    }
}
