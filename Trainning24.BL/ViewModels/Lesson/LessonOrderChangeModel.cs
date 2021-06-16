using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Lesson
{
    public class LessonOrderChangeModel
    {
        public LessonOrderDetial previousdetail { get; set; }
        public LessonOrderDetial newdetail { get; set; }
    }


    public class LessonOrderDetial
    {
        public long chapterid { get; set; }
        public List<LessonDetail> lessondetaillist { get; set; }
    }


    public class LessonDetail
    {
        public long id { get; set; }
        public int itemorder { get; set; }
        public int type { get; set; }
    }
}
