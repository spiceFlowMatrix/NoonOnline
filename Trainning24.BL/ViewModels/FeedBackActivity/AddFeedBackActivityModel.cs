using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.FeedBackActivity
{
    public class AddFeedBackActivityModel
    {
        public long id { get; set; }
        public long FeedbackId { get; set; }
        public long FeedbackTaskId { get; set; }
        public long UserId { get; set; }
        public long Type { get; set; }
    }
}
