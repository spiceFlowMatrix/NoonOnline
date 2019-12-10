using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.ProgressSyncDTO
{
    public class ChapterProgressDTO
    {
        public long courseid { get; set; }
        public long chapterid { get; set; }
        public long userid { get; set; }
        public long progress { get; set; }
    }
}
