using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Chapter
{
    public class ResponseChapterModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long Courseid { get; set; }
        public long? quizid { get; set; }
        public int itemorder { get; set; }

        public List<AssignmentDetailModel> assignmentDetails { get; set; }
    }


    public class AssignmentDetailModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
    }


    public class ResponseChapterModelByCourseid
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public long CourseId { get; set; }
        public int ItemOrder { get; set; }
    }


    public class ResponseChapter
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
