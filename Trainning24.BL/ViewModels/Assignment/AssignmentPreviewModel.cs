using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.AssignmentFile;

namespace Trainning24.BL.ViewModels.Assignment
{
    public class AssignmentPreviewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int itemorder { get; set; }
        public int type { get; set; }
        public List<ResponseAssignmentFileModel> assignmentfiles { get; set; }
    }
}
