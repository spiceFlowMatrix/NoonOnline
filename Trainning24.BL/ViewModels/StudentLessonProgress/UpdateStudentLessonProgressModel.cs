using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.StudentLessonProgress
{
    public class UpdateStudentLessonProgressModel
    {
        public long id { get; set; }
        public long lessonid { get; set; }
        public long lessonstatus { get; set; }
        public long studentid { get; set; }
        public decimal lessonprogress { get; set; }
        //public long duration { get; set; }
    }
}
