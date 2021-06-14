using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.AssignmentFile;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.LessonFile;

namespace Trainning24.BL.ViewModels.Assignment
{
    public class ResponseAssignmentModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }        

        public ResponseChapterModel chapter { get; set; }
        public List<ResponseAssignmentFileModel> assignmentfiles { get; set; }
    }



    public class AssignmentDetails
    {
        public long id { get; set; }
        public string name { get; set; }
        public CourseDetailModel coursewithgrade { get; set; }
        //public string description { get; set; }
        //public string code { get; set; }

        //public ResponseChapterModel chapter { get; set; }
        //public List<ResponseAssignmentFileModel> assignmentfiles { get; set; }
    }
}
