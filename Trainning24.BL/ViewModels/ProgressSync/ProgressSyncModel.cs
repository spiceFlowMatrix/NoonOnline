using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.ProgressSync
{
    public class ProgressSyncModel
    {
        public long? GradeId { get; set; }
        public bool IsStatus { get; set; }
        public long? LessonProgressId { get; set; }
        public long? LessonId { get; set; }
        public decimal? LessonProgress { get; set; }
        public long? QuizId { get; set; }
        public long? TotalRecords { get; set; }
        public long UserId { get; set; }
        public long FileId { get; set; }
    }

    public class TimerSyncModel
    {
        public bool isStatus { get; set; }
        public string passingScore { get; set; }
        public long quizId { get; set; }
        public string quizTime { get; set; }
        public long userId { get; set; }
        public string yourScore { get; set; }
    }

    public class SyncDataResponse
    {
        public List<ProgressSyncModel> progressdata { get; set; }
        public List<TimerSyncModel> timerdata { get; set; }
    }
}
