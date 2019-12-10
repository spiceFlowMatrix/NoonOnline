using System.Collections.Generic;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.LessonAssignment;
using Trainning24.BL.ViewModels.LessonFile;

namespace Trainning24.BL.ViewModels.Lesson
{
    public class ResponseLessonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public int itemorder { get; set; }
        public ResponseChapterModel chapter { get; set; }
        public List<ResponseLessonFileModel> lessonfiles { get; set; }
        public ResponseLessionAssignmentDTO assignment { get; set; }
    }


    public class ResponseLessonModelByChapter
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
    }

    public class LessonDetailModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public string coursename { get; set; }
        public long chapterid { get; set; }
        public long courseid { get; set; }
        public long lessonfileid { get; set; }
        public string lessonfilename { get; set; }
        public long filetypeid { get; set; }
        public string filetypename { get; set; }
        public decimal progressval { get; set; }
        public long lessonquizid { get; set; }
    }

}
