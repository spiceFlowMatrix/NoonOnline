using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Assignment
{
    public class AddAssignmentModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public long chapterid { get; set; }
        public List<AddAssignmentFileModelDTO> assignmentfiles { get; set; }
    }

    public class CourseAssignmentModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public long chapterid { get; set; }
        public long submissioncount { get; set; } 
        public long status { get; set; }
    }

    public class AddAssignmentFileModelDTO
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string filetypename { get; set; }
        public long filetypeid { get; set; }
    }
}
