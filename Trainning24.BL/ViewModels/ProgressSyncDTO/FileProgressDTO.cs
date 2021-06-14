using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.ProgressSyncDTO
{
    public class FileProgressDTO
    {
        public long lessonid { get; set; }
        public long fileid { get; set; }
        public long userid { get; set; }
        public long progress { get; set; }
    }
}
