using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.ProgressSyncDTO
{
    public class QuizChartDTO
    {
        public long id { get; set; }
        public long progress { get; set; }
        public DateTime datetime { get; set; }
    }

    public class QuizPieChartDTO
    {
        public long id { get; set; }
        public long quizid { get; set; }
        public long progress { get; set; }
        public DateTime datetime { get; set; }
        public long passmark { get; set; }
    }

    public class AssinmentPieChartDTO
    {
        public long id { get; set; }
        public long assignmentid { get; set; }
        public long progress { get; set; }
        public DateTime datetime { get; set; }
    }

    public class AllProgressPieChartDTO
    {
        public long progress { get; set; }
        public DateTime datetime { get; set; }
    }

    public class LessonPieChartDTO
    {
        public long id { get; set; }
        public long lessonid { get; set; }
        public long progress { get; set; }
        public DateTime datetime { get; set; }
    }
}
