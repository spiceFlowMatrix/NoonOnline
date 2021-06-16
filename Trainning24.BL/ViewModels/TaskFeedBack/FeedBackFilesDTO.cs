using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;

namespace Trainning24.BL.ViewModels.TaskFeedBack
{
    public class FeedBackFilesDTO
    {
        public long id { get; set; }
        public ResponseFilesModel files { get; set; }
    }
}
