using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.LessonAssignment
{
    public class AddLessionAssignmentDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public long lessonid { get; set; }
        public List<int> files { get; set; }
    }
}
