using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.LessonAssignment;

namespace Trainning24.BL.ViewModels.Lesson
{
    public class AddLessonDTO
    {
        public long chapterid { get; set; }
        public AddLessonModelDTO lesson { get; set; }
    }

    public class AddLessonModelDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public List<AddLessonFileModelDTO> files { get; set; }
        public AddLessionAssignmentDTO assignment { get; set; }
    }
    public class AddLessonFileModelDTO
    {
        public long Id { get; set; }
        public string name { get; set; }
        public string filetypename { get; set; }
        public long filetypeid { get; set; }
    }
}
