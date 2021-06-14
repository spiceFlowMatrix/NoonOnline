using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.ProgressSyncDTO
{
    public class UserQuizResultDTO
    {
        public long quizid { get; set; }
        public long userid { get; set; }
        public long totalquestion { get; set; }
        public long answeredquestion { get; set; }
        public string performdate { get; set; }
        public long passingscore { get; set; }
        public long score { get; set; }
    }
}
