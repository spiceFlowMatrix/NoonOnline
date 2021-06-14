using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.LessonAssignment;
using Trainning24.BL.ViewModels.LessonFile;

namespace Trainning24.BL.ViewModels.Course
{
    public class LessonPrivewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int itemorder { get; set; }
        public int type { get; set; }
        public List<ResponseLessonFileModel> lessonfiles { get; set; }
        public ResponseLessionAssignmentDTO assignment { get; set; }

    }
}
