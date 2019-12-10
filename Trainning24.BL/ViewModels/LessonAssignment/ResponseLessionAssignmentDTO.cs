using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.LessonAssignmentFile;

namespace Trainning24.BL.ViewModels.LessonAssignment
{
    public class ResponseLessionAssignmentDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public List<ResponseLessonAssignmentFileDTO> assignmentfiles { get; set; }
    }
}
