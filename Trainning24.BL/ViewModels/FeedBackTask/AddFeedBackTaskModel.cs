using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.FeedBackTask
{
    public class AddFeedBackTaskModel
    {
        public long id { get; set; }
        public long feedbackid { get; set; }
        //public long userid { get; set; }
        public string description { get; set; }
        public string filelink { get; set; }
        public long type { get; set; }
    }
}
